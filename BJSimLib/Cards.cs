using System;
using System.Collections.Generic;
using System.Text;

namespace BJSimLib
{
    public class Cards
    {
		public enum SUIT
		{
			SPADES, HEARTS, CLUBS, DIAMONDS
		}

		public enum FACEVALUE  // Used for card facevalue visual display
		{
			TWO = 2, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE
		}

		public enum VALUE  // Used for card value for scoring (tens and face cards all equal 10)
		{
			TWO = 2, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, ELEVEN
		}

		public SUIT MySuit { get; set; }
		public VALUE MyValue { get; set; }
		public FACEVALUE MyFaceValue { get; set; }
	}
}
