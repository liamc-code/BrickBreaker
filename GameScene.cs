/* GameScene.cs
* BrickBreaker
* Revision History
* Liam Conn, 2024.11.22: Created
* Liam Conn, 2024.11.24: Added Code
* Liam Conn, 2024.12.08: Debugging complete
* Liam Conn, 2024.12.08: Comments added
*
*/

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    /// <summary>
    /// Base class for game scenes
    /// </summary>
    public abstract class GameScene : DrawableGameComponent
    {
        public List<GameComponent> Components { get; set; }

        /// <summary>
        /// Hide scene by disabiling visibility/updates
        /// </summary>
        public virtual void HideScene()
        {
            this.Visible = false;
            this.Enabled = false;
        }

        /// <summary>
        /// Show scene by enabling visibility/updates
        /// </summary>
        public virtual void ShowScene()
        {
            this.Visible = true;
            this.Enabled = true;
        }

        protected GameScene(Game game) : base(game)
        {
            Components = new List<GameComponent>();
            HideScene();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent gameComponent in Components)
            {
                if (gameComponent.Enabled)
                {
                    gameComponent.Update(gameTime);
                }
            }
            base.Update(gameTime);  
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(GameComponent gameComponent in Components)
            {
                if(gameComponent is DrawableGameComponent)
                {
                    DrawableGameComponent drawableGameComponent = (DrawableGameComponent)gameComponent;
                    if(drawableGameComponent.Visible)
                    {
                        drawableGameComponent.Draw(gameTime);
                    }
                }
            }
            base.Draw(gameTime);
        }
    }
}
