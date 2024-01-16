#region Namespaces

using BlazorGE.Game;
using BlazorGE.Game.Components;
using BlazorGE.Graphics2D.Services;
using BlazorGE.Input;
using System.Threading.Tasks;

#endregion

namespace DemoGame.Game.Components
{
    public class PlayerMovementComponent : GameComponentBase, IUpdatableGameComponent
    {
        #region Protected Properties

        protected IGraphicsService2D GraphicsService2D;
        protected IMouseService MouseService;
        protected IKeyboardService KeyboardService;

        #endregion

        #region Public Properties

        public float Speed;

        private bool IsMoving { get; set; }

        private MouseState LatestMouseState { get; set; }

        #endregion

        #region Constructors

        public PlayerMovementComponent(IKeyboardService keyboardService, 
            IGraphicsService2D graphicsService2D, 
            IMouseService mouseService, 
            float initialSpeed = 0.25f)
        {
            KeyboardService = keyboardService;
            GraphicsService2D = graphicsService2D;
            MouseService = mouseService;
            Speed = initialSpeed;
        }

        #endregion

        #region Implementations

        /// <summary>
        /// Updates the player
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public async ValueTask UpdateAsync(GameTime gameTime)
        {
            var spriteComponent = GameEntityOwner.GetComponent<SpriteComponent>();

            // Get the current state of keyboard
            var kstate = KeyboardService.GetState();
            var mouseState = MouseService.GetState();

            // Get the transform component
            var transformComponent = GameEntityOwner.GetComponent<Transform2DComponent>();
            var direction = transformComponent.Direction;

            // Do we move left/right?
            if (kstate.IsKeyDown(Keys.LeftArrow)) direction.X = -1;
            else if (kstate.IsKeyDown(Keys.RightArrow)) direction.X = 1;

            // Do we move up/down?
            if (kstate.IsKeyDown(Keys.UpArrow)) direction.Y = -1;
            else if (kstate.IsKeyDown(Keys.DownArrow)) direction.Y = 1;

            if (!IsMoving && mouseState.KeyState == KeyState.Down 
                          && transformComponent.IntersectsWith((int)mouseState.X, (int)mouseState.Y))
            {

                IsMoving = true;
            }
            else if (IsMoving && mouseState.KeyState == KeyState.Up)
            {
                IsMoving = false;
            }

            //Cancel move if mouse is outside of canvas
            if (IsMoving && (mouseState.X < 0 || mouseState.X > GraphicsService2D.CanvasWidth
                                              || mouseState.Y < 0 || mouseState.Y > GraphicsService2D.CanvasHeight))
            {
                IsMoving = false;
            }
            


            if (IsMoving)
            {
                transformComponent.Position.X = (int)(mouseState.X - spriteComponent.Sprite.Width / 2);
                transformComponent.Position.Y = (int)(mouseState.Y - spriteComponent.Sprite.Height / 2);
            }

            // Move this entity         
            transformComponent.Translate(direction * Speed * gameTime.TimeSinceLastFrame);

            // Check we've not gone out of bounds
            if (transformComponent.Position.X < 0) transformComponent.Position.X = 0;
            else if (transformComponent.Position.X > GraphicsService2D.CanvasWidth - spriteComponent.Sprite.Width)
                transformComponent.Position.X = GraphicsService2D.CanvasWidth - spriteComponent.Sprite.Width;

            if (transformComponent.Position.Y < 0) transformComponent.Position.Y = 0;
            else if (transformComponent.Position.Y > GraphicsService2D.CanvasHeight - spriteComponent.Sprite.Height)
                transformComponent.Position.Y = GraphicsService2D.CanvasHeight - spriteComponent.Sprite.Height;

            await Task.CompletedTask;
        }

        #endregion
    }
}
