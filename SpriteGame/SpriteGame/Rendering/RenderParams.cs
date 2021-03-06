using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SpriteGame.Rendering
{
    // TODO: pop/push matrix, translate, rotate, scale, etc..., identity
    /// <summary>
    /// A class holding important rendering parameters, such as the view and projection matrices.
    /// Allows temporary modification of those matrices, e.g. for hierachical rendering.
    /// </summary>
    public class RenderParams
    {
        /// <summary>
        /// A stack of matrices, saving previous transformation state.
        /// This is used to implement hierachical rendering.
        /// </summary>
        protected Stack<Matrix4> matrixStack = new Stack<Matrix4>();

        /// <summary>
        /// The view matrix that should be used for this rendering job
        /// </summary>
        public Matrix4 View { get; protected set; }

        /// <summary>
        /// The projection matrix that should be used for this rendering job
        /// </summary>
        public Matrix4 Projection { get; protected set; }

        /// <summary>
        /// The current model transformation matrix. This can be freely changed by rendering
        /// code. Can be saved by using <see cref="PushMatrix"/> in order to implement
        /// hierachical rendering.
        /// </summary>
        public Matrix4 Model { get; set; }

        /// <summary>
        /// Create a new set of rendering parameters using given view and projection matrices.
        /// The model matrix is initialized to the identity matrix.
        /// </summary>
        /// <param name="view">View matrix</param>
        /// <param name="projection">Projection matrix</param>
        public RenderParams(Matrix4 view, Matrix4 projection)
        {
            View = view;
            Projection = projection;
            Model = Matrix4.Identity;
        }

        /// <summary>
        /// Apply stored Model, View and Projection matrices to given program
        /// </summary>
        /// <param name="p"></param>
        public void ApplyToProgram(ShaderProgram p)
        {
            p.Projection = Projection;
            p.Model = Model;
            p.View = View;
        }

        /// <summary>
        /// Save the currently held transformation state to the stack.
        /// </summary>
        public void PushMatrix()
        {
            matrixStack.Push(Model);
        }

        /// <summary>
        /// Restore the topmost stack element as the current model transformation matrix
        /// </summary>
        public void PopMatrix()
        {
            if (matrixStack.Count == 0)
                throw new InvalidOperationException("Tried to pop matrix from empty matrix stack");

            Model = matrixStack.Pop();
        }

        /// <summary>
        /// Add a translation to the model matrix
        /// </summary>
        /// <param name="v">Vector to translate by</param>
        public void Translate(Vector3 v)
        {
            var mat = Matrix4.CreateTranslation(v);
            Model = Model * mat;
        }

        /// <summary>
        /// Add a translation to the model matrix
        /// </summary>
        /// <param name="x">X component of the translation</param>
        /// <param name="y">Y component of the translation</param>
        /// <param name="z">Z component of the translation</param>
        public void Translate(float x, float y, float z)
        {
            this.Translate(new Vector3(x, y, z));
        }

        /// <summary>
        /// Add a scale operation to the model matrix
        /// </summary>
        /// <param name="x">Scaling in X direction</param>
        /// <param name="y">Scaling in Y direction</param>
        /// <param name="z">Scaling in Z direction</param>
        public void Scale(float x, float y, float z)
        {
            var mat = Matrix4.CreateScale(x, y, z);
            Model = Model * mat;
        }

        /// <summary>
        /// Add a uniform scale operation to the model matrix
        /// </summary>
        /// <param name="f">Scaling factor</param>
        public void Scale(float f)
        {
            var mat = Matrix4.CreateScale(f);
            Model = Model * mat;
        }

        /// <summary>
        /// Add a rotation around the X axis to the model matrix
        /// </summary>
        /// <param name="angle">Rotation angle, in radians</param>
        public void RotateX(float angle)
        {
            var mat = Matrix4.CreateRotationX(angle);
            Model = Model * mat;
        }

        /// <summary>
        /// Add a rotation around the Y axis to the model matrix
        /// </summary>
        /// <param name="angle">Rotation angle, in radians</param>
        public void RotateY(float angle)
        {
            var mat = Matrix4.CreateRotationY(angle);
            Model = Model * mat;
        }

        /// <summary>
        /// Add a rotation around the Z axis to the model matrix
        /// </summary>
        /// <param name="angle">Rotation angle, in radians</param>
        public void RotateZ(float angle)
        {
            var mat = Matrix4.CreateRotationZ(angle);
            Model = Model * mat;
        }

        /// <summary>
        /// Add a rotation around the an arbitrary axis to the model matrix
        /// </summary>
        /// <param name="axis">Roation axis</param>
        /// <param name="angle">Rotation angle, in radians</param>
        public void Rotate(Vector3 axis, float angle)
        {
            var mat = Matrix4.CreateFromAxisAngle(axis, angle);
            Model = Model * mat;
        }
    }
}
