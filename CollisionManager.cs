/* CollisionManager.cs
* BrickBreaker
* Revision History
* Liam Conn, 2024.11.22: Created
* Liam Conn, 2024.11.27  Added Code
* Liam Conn, 2024.11.08: Added Code
* Liam Conn, 2024.12.08: Debugging complete
* Liam Conn, 2024.12.08: Comments added
*
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    /// <summary>
    /// Handle collision detection and respond for objects including ball,
    /// paddle, bricks
    /// </summary>
    public class CollisionManager : GameComponent
    {
        // Game entities
        private Ball _ball;
        private Paddle _paddle;
        private List<Brick> _bricks;
        private PlayScene _playScene;

        // Sound effects
        private SoundEffect _ballBounce;
        private SoundEffect _brickHit;

        /// <summary>
        /// New instance of class
        /// </summary>
        /// <param name="game"></param>
        /// <param name="playScene">PlayScene object for scores</param>
        /// <param name="ball">Ball object for collision checks</param>
        /// <param name="paddle">Paddle object for collisions</param>
        /// <param name="bricks">List of bricks for collisions</param>
        /// <param name="ballBounce">ball bounce sound</param>
        /// <param name="brickHit">brick hit sound</param>
        public CollisionManager(Game game, PlayScene playScene, Ball ball, Paddle paddle, List<Brick> bricks,
            SoundEffect ballBounce, SoundEffect brickHit) : base(game)
        {
            _playScene = playScene; // Reference for PlayScene
            _ball = ball;
            _paddle = paddle;
            _bricks = bricks;

            // sounds
            _ballBounce = ballBounce;
            _brickHit = brickHit;

        }

        /// <summary>
        /// Update collision reactions each frame
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Collision with paddle
            if (_ball.GetBounds().Intersects(_paddle.GetBounds()))
            {
                _ball.ReflectY();
                _ball.NudgeY(5);

                // Play ball bounce
                _ballBounce.Play();
            }
                

            // Collision with Bricks
            foreach(Brick brick in _bricks)
            {
                if(brick.IsVisible && _ball.GetBounds().Intersects(brick.GetBounds()))
                {
                    brick.TakeHit();
                    _ball.ReflectY();
                    _ball.NudgeY(5);

                    // play brick hit sound
                    _brickHit.Play();

                    if(!brick.IsVisible)
                    {
                        _playScene.AddScore(10);
                        // remove brick from list of bricks
                        _bricks.Remove(brick);
                    } else
                    {
                        _playScene.AddScore(5);
                    }

                    break;
                }
            }

            base.Update(gameTime);
        }
    }
}
