using System.Collections.Generic; // List<>
using System.Numerics; // Vector2

namespace RayDot
{
	abstract class Node : ITransformable, IUpdatable
	{
		// Data structure
		private ITransformable parent;
		private List<ITransformable> children;
		public List<ITransformable> Children {
			get { return children; }
		}
		public ITransformable Parent {
			get { return parent; }
			set { parent = value; }
		}

		// Transform
		protected Vector2 position;
		protected float rotation;
		protected Vector2 scale;

		public Vector2 Position {
			get { return position; }
			set { position = value; }
		}
		public float Rotation {
			get { return rotation; }
			set { rotation = value; }
		}
		public Vector2 Scale {
			get { return scale; }
			set { scale = value; }
		}

		// TransformNode sets these values after transform.
		private Vector2 worldPosition;
		private float worldRotation;
		private Vector2 worldScale;

		public Vector2 WorldPosition {
			get { return worldPosition; }
		}
		public float WorldRotation {
			get { return worldRotation; }
		}
		public Vector2 WorldScale {
			get { return worldScale; }
		}

		// Constructor
		protected Node()
		{
			this.children = new List<ITransformable>();
			Parent = null;
			Position = new Vector2(0.0f, 0.0f);
			Rotation = 0.0f;
			Scale = new Vector2(1.0f, 1.0f);
		}

		public virtual void Update(float deltaTime)
		{
			// virtual (override in subclass)
			// or don't, then this will be called
		}

		public bool AddChild(ITransformable child)
		{
			if (children.Contains(child))
			{
				// this is already our child
				return false;
			}
			if (child == this)
			{
				// this is us! we can't be our own child.
				return false;
			}
			if (child.Parent != null) // handle previous owner
			{
				// "kidnap" the child from previous parent
				child.Parent.RemoveChild(child, false);
			}
			child.Parent = this;
			children.Add(child);
			return true;
		}

		public bool RemoveChild(ITransformable child, bool keepAlive = false)
		{
			// we don't know this child
			if (!children.Contains(child))
			{
				return false;
			}

			// do we need to keep this child alive?
			if (keepAlive)
			{
				// pass back up to our parent
				if (this.parent == null)
				{
					// we're the scene, we have no parents
					return false;
				}
				child.Parent = this.parent;
				child.Parent.AddChild(child);
			}

			// remove from our children
			children.Remove(child);
			return true;
		}

		public void TransformNode(Matrix4x4 parentMatrix)
		{
			// ========== Transform all nodes ==========
			// locals (we need Vec3 to use with Mat4 in System.Numerics)
			Vector3 position = new Vector3(Position, 0.0f);
			Vector3 rotation = new Vector3(0.0f, 0.0f, Rotation);
			Vector3 scale = new Vector3(Scale, 0.0f);

			// build individual translation, rotation and scale Matrices
			Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(position);
			Matrix4x4 rotMatZ = Matrix4x4.CreateRotationZ(rotation.Z);
			// Matrix4x4 rotMatX = Matrix4x4.CreateRotationX(rotation.X);
			// Matrix4x4 rotMatY = Matrix4x4.CreateRotationX(rotation.Y);
			// Matrix4x4 rotationMatrix = rotMatX * rotMatY * rotMatZ;
			Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scale);

			// build modelMatrix for this Entity
			// Matrix4x4 modelMatrix = scaleMatrix * rotationMatrix * translationMatrix;
			Matrix4x4 modelMatrix = scaleMatrix * rotMatZ * translationMatrix;

			// multiply with parent
			modelMatrix *= parentMatrix;

			// extract world coords
			Vector3 worldpos;
			Quaternion worldrotQ;
			Vector3 worldscl;
			Matrix4x4.Decompose(modelMatrix, out worldscl, out worldrotQ, out worldpos);

			// set World coords
			worldPosition = new Vector2(worldpos.X, worldpos.Y);
			worldRotation = Vector3.Transform(rotation, worldrotQ).Z; // TODO check
			// Rotation is not inherited from parent. For now, we hack it in.
			if (Parent != null) {
				worldRotation = Parent.WorldRotation + this.rotation;
			}
			worldScale = new Vector2(worldscl.X, worldscl.Y);

			// transform all children
			for (int i=0; i<children.Count; i++)
			{
				children[i].TransformNode(modelMatrix);
			}
		}

		// "internal" method to be called from Core
		// Updates all children recursively
		public void UpdateNode(float deltaTime)
		{
			// IUpdatable
			Update(deltaTime);

			// A Node is IUpdatable: update all children
			for (int i=0; i<children.Count; i++)
			{
				((Node)children[i]).UpdateNode(deltaTime);
			}
		}

	}
}
