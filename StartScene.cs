/* StartScene.cs
* BrickBreaker
* Revision History
* Liam Conn, 2024.11.22: Created
* Liam Conn, 2024.11.24: Added Code
* Liam Conn, 2024.11.27  Added Code
* Liam Conn, 2024.11.08: Added Code
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
    /// Represent start menu scene where player can navigate through options.
    /// </summary>
    public class StartScene : GameScene
    {
        private MenuComponent _menu;
        public MenuComponent Menu { get => _menu; set => _menu = value;}

        public StartScene(Game game) : base(game)
        {
            Game1 game1 = (Game1)game;
            // Menu options
            string[] menuOptions = { "Play Game!", "Score Board", "Help", "About", "Quit" };

            SpriteFont normalFont = game1.Content.Load<SpriteFont>("fonts/NormalFont");
            SpriteFont selectedFont = game1.Content.Load<SpriteFont>("fonts/SelectedFont");

            Menu = new MenuComponent(game, game1._spriteBatch, normalFont, selectedFont, menuOptions);
            this.Components.Add(Menu);
        }
    }
}
