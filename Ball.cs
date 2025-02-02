/* Ball.cs
* BrickBreaker
* Revision History
* Liam Conn, 2024.11.22: Created
* Liam Conn, 2024.11.24: Added Code
* Liam Conn, 2024.12.08: Added Code
* Liam Conn, 2024.12.08: Debugging complete
* Liam Conn, 2024.12.08: Comments added
*
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    /// <summary>
    /// Ball object class, handle movement, collision, reset 
    /// </summary>
    public class Ball : DrawableGameComponent
    {
        // Ball properties
        private SpriteBatch _sbBall;
        private Texture2D _ballTexture;
        private Vector2 _ballPosition;
        private Vector2 _ballSpeed;
        private float _ballScale;

        // Ball event
        public event Action OnBallMiss; // event to trigger game over

        /// <summary>
        /// New instance of Ball class
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="texture">Texture for ball</param>
        public Ball(Game game, SpriteBatch spriteBatch, Texture2D texture) : base(game)
        {
            _sbBall = spriteBatch;
            _ballTexture = texture;
            _ballPosition = new Vector2(SharedScene.sharedStage.X/2, SharedScene.sharedStage.Y/2);
            _ballSpeed = new Vector2(3, -3);
            _ballScale = 0.3f;
        }

        /// <summary>
        /// Update ball position/check for collisions
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _ballPosition += _ballSpeed;

            // Ball bounces off a wall
            if (_ballPosition.X <= 0 || _ballPosition.X >= SharedScene.sharedStage.X - (_ballTexture.Width * _ballScale))
                _ballSpeed.X = -_ballSpeed.X;
            
            if(_ballPosition.Y <= 0)
                _ballSpeed.Y = -_ballSpeed.Y;

            // Ball misses below bottom of screen
            if(_ballPosition.Y >= SharedScene.sharedStage.Y)
                OnBallMiss?.Invoke(); // Trigger event

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw ball on screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            _sbBall.Draw(_ballTexture, _ballPosition, null, Color.White, 0f, Vector2.Zero, _ballScale, SpriteEffects.None,
                0f);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Changes direction of Y velocity to be opposite
        /// </summary>
        public void ReflectY()
        {
            _ballSpeed.Y = -_ballSpeed.Y;
        }

        /// <summary>
        /// Bounding rectangle for collision detection
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBounds()
        {
            return new Rectangle((int)_ballPosition.X, (int)_ballPosition.Y,
                (int)(_ballTexture.Width * _ballScale), (int)(_ballTexture.Height * _ballScale));
        }

        /// <summary>
        /// Avoid getting stuck by bouncing it a bit during collision
        /// </summary>
        /// <param name="amount"></param>
        public void NudgeY(float amount)
        {
            _ballPosition.Y += (_ballSpeed.Y > 0) ? amount : -amount;
        }

        /// <summary>
        /// Reset ball to starting position and speed
        /// </summary>
        public void ResetPosition()
        {
            _ballPosition = new Vector2(SharedScene.sharedStage.X / 2, SharedScene.sharedStage.Y - 50);
            _ballSpeed = new Vector2(3, -3);
        }
    }
}
