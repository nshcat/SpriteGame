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
        /// Modify the model transformation matrix by applying a translation by given vector to it.
        /// </summary>
        /// <param name="v">Vector to translate by</param>
        public void Translate(Vector3 v)
        {
            var mat = Matrix4.CreateTranslation(v);
            Model = Model * mat;
        }
    }
}
