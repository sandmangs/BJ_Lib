using System;
using System.Collections.Generic;
using System.Text;

namespace BJSimLib
{
    public class CardDecks : Cards
    {
		public int nextCardIndex = 0;
		const int NUM_OF_CARDS = 52;
		private Cards[] deck;
		public int cardMark = 0;
		public int decks = 0;
		public int numberOfShuffles = 0;

		public Cards[] GetDeck { get { return deck; } }

		public void SetUpDeck(int cardDecks)
		{
			decks = cardDecks;
			deck = new Cards[decks * 52];
			int index = 0;

			System.Threading.Thread.Sleep(25);

			for (int i = 0; i < cardDecks; i++)
			{
				foreach (SUIT s in Enum.GetValues(typeof(SUIT)))
				{
					foreach (FACEVALUE v in Enum.GetValues(typeof(FACEVALUE)))
					{
						switch (v)
						{
							case FACEVALUE.TWO:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.TWO };
								break;
							case FACEVALUE.THREE:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.THREE };
								break;
							case FACEVALUE.FOUR:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.FOUR };
								break;
							case FACEVALUE.FIVE:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.FIVE };
								break;
							case FACEVALUE.SIX:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.SIX };
								break;
							case FACEVALUE.SEVEN:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.SEVEN };
								break;
							case FACEVALUE.EIGHT:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.EIGHT };
								break;
							case FACEVALUE.NINE:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.NINE };
								break;
							case FACEVALUE.TEN:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.TEN };
								break;
							case FACEVALUE.JACK:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.TEN };
								break;
							case FACEVALUE.QUEEN:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.TEN };
								break;
							case FACEVALUE.KING:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.TEN };
								break;
							case FACEVALUE.ACE:
								deck[index] = new Cards { MySuit = s, MyFaceValue = v, MyValue = VALUE.ELEVEN };
								break;
						}
						index++;
					}
				}
			}
			ShuffleCards();
			System.Threading.Thread.Sleep(0);
		}

		public void ShuffleCards()
		{
			Random rand = new Random();
			Cards temp;

			numberOfShuffles++;
			int tempvar = (int)(NUM_OF_CARDS * decks * .15);
			cardMark = NUM_OF_CARDS * decks - (int)(NUM_OF_CARDS * decks * .1) - rand.Next(tempvar);
			nextCardIndex = 0;
			for (int shuffleTimes = 0; shuffleTimes < 1000; shuffleTimes++)
			{
				for (int i = 0; i < NUM_OF_CARDS * decks; i++)
				{
					int secondCardIndex = rand.Next(NUM_OF_CARDS * decks);
					temp = deck[i];
					deck[i] = deck[secondCardIndex];
					deck[secondCardIndex] = temp;
				}
			}

			// Below used for testing purposes.  Uncomment and change card value and placement to test

			for (int i = 0; i < NUM_OF_CARDS; i++)
			{
				if ((int)deck[i].MyFaceValue == 14)
				{
					int secondCardIndex = 0;
					temp = deck[i];
					deck[i] = deck[secondCardIndex];
					deck[secondCardIndex] = temp;
					break;
				}
			}
			//for (int i = 0; i < NUM_OF_CARDS; i++)
			//{
			//	if ((int)deck[i].MyFaceValue == 10)
			//	{
			//		int secondCardIndex = 2;
			//		temp = deck[i];
			//		deck[i] = deck[secondCardIndex];
			//		deck[secondCardIndex] = temp;
			//		break;
			//	}
			//}
			int tempr = 0;
			for (int i = 0; i < NUM_OF_CARDS; i++)
			{
				if ((int)deck[i].MyFaceValue == 14)
				{
					if (tempr == 1)
					{
						int secondCardIndex = 2;
						temp = deck[i];
						deck[i] = deck[secondCardIndex];
						deck[secondCardIndex] = temp;
						break;
					}
					tempr = 1;
				}
			}
			//tempr = 0;
			//for (int i = 0; i < NUM_OF_CARDS; i++)
			//{
			//	if ((int)deck[i].MyFaceValue == 10)
			//	{
			//		if (tempr == 1)
			//		{
			//			int secondCardIndex = 3;
			//			temp = deck[i];
			//			deck[i] = deck[secondCardIndex];
			//			deck[secondCardIndex] = temp;
			//			break;
			//		}
			//		tempr = 1;
			//	}
			//}
		}
	}
}

