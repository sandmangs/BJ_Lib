using System;
using System.Collections.Generic;
using System.Text;

namespace BJSimLib
{
    public class DrawCards
    {
		//draw cards outlines
		public static void DrawCardOutline(int xcoor, int ycoor)
		{
			Console.ForegroundColor = ConsoleColor.Black;

			int x = xcoor * 12;
			int y = ycoor;

			Console.SetCursorPosition(x, y);
			Console.Write(" __________\n"); //top edge of the card

			for (int i = 0; i < 10; i++)
			{
				Console.SetCursorPosition(x, y + 1 + i);

				if (i != 9)
					Console.WriteLine("|          |");//left and right edges of the card
				else
					Console.WriteLine("|__________|");//bottom edge of the card
			}
		}

		//draw cards outline / back of card
		public static void DrawCardOutlineBack(int xcoor, int ycoor)
		{
			Console.ForegroundColor = ConsoleColor.Black;

			int x = xcoor * 12;
			int y = ycoor;

			Console.SetCursorPosition(x, y);
			Console.Write(" __________\n"); //top edge of the card

			for (int i = 0; i < 10; i++)
			{
				Console.SetCursorPosition(x, y + 1 + i);

				if (i != 9)
				{
					if (i == 0)
					{
						Console.WriteLine("|          |");//back of card
					}
					if (i == 2 || i == 4 || i == 6 || i == 8)
					{
						Console.WriteLine("| ^ ^ ^ ^ ^|");//back of card
					}
					if (i == 1 || i == 3 || i == 5 || i == 7)
					{
						Console.WriteLine("|^ ^ ^ ^ ^ |");//back of card
					}
				}
				else
				{
					Console.WriteLine("|__________|");//bottom edge of the card
				}
			}
		}

		//displays suit and value of the card inside its outline
		public static void DrawCardSuitValue(Cards card, int xcoor, int ycoor)
		{
			char cardSuit = ' ';
			int x = xcoor * 12;
			int y = ycoor;

			//hearts and diamonds are red, clubs and spades are black
			switch (card.MySuit)
			{
				case Cards.SUIT.HEARTS:
					cardSuit = '♥';
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case Cards.SUIT.DIAMONDS:
					cardSuit = '♦';
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case Cards.SUIT.CLUBS:
					cardSuit = '♣';
					Console.ForegroundColor = ConsoleColor.Black;
					break;
				case Cards.SUIT.SPADES:
					cardSuit = '♠';
					Console.ForegroundColor = ConsoleColor.Black;
					break;
			}

			//display the encoded character and value of the card
			Console.SetCursorPosition(x + 5, y + 5);
			Console.Write(cardSuit);
			Console.SetCursorPosition(x + 4, y + 7);
			Console.Write(card.MyFaceValue);

		}
	}
}
