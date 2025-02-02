/* Paddle.cs
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
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    /// <summary>
    /// Paddle in game, controlled by player
    /// </summary>
    public class Paddle : DrawableGameComponent
    {
        private SpriteBatch _sbPaddle;
        private Texture2D _paddleTexture;
        private Vector2 _paddlePosition;
        private Vector2 _paddleSpeed;
        private float _paddleScale;

        /// <summary>
        /// New instance of Paddle class
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        public Paddle(Game game, SpriteBatch spriteBatch, Texture2D texture) : base(game)
        {
            _sbPaddle = spriteBatch;
            _paddleTexture = texture;
            _paddleSpeed = new Vector2(5, 0);
            _paddleScale = 0.4f;
            _paddlePosition = new Vector2(SharedScene.sharedStage.X/2-(_paddleTexture.Width * _paddleScale)/2, 
                SharedScene.sharedStage.Y-(_paddleTexture.Height*_paddleScale)-10);
        }

        /// <summary>
        /// update paddle's position based on player input.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            // move paddle left or right in screen boundary
            if(kState.IsKeyDown(Keys.Left))
            {
                _paddlePosition.X -= _paddleSpeed.X;
                if(_paddlePosition.X < 0 )
                    _paddlePosition.X = 0;
            }

            if(kState.IsKeyDown(Keys.Right))
            {
                _paddlePosition.X += _paddleSpeed.X;
                if (_paddlePosition.X > SharedScene.sharedStage.X - (_paddleTexture.Width*_paddleScale))
                    _paddlePosition.X = SharedScene.sharedStage.X - (_paddleTexture.Width * _paddleScale);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw paddle on screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            _sbPaddle.Draw(_paddleTexture, _paddlePosition, null, Color.White, 0f, Vector2.Zero, _paddleScale,
                SpriteEffects.None, 0f);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Get bounding rectangle for collisions
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBounds()
        {
            return new Rectangle((int)_paddlePosition.X, (int)_paddlePosition.Y, 
                (int)(_paddleTexture.Width * _paddleScale), (int)(_paddleTexture.Height * _paddleScale)); 
        }

        /// <summary>
        /// Reset paddle to start position
        /// </summary>
        public void ResetPosition()
        {
            _paddlePosition = new Vector2(SharedScene.sharedStage.X/2 - _paddleTexture.Width/2,
                SharedScene.sharedStage.Y - 30);
        }
    }
}
