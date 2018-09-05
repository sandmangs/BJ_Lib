using System;
using System.Collections.Generic;
using System.Text;

namespace BJSimLib
{
    public class PlayerModel : CardDecks
    {
		public Cards[] hand;
		public Cards[] hand2;

		public List<string> startingHandTotals = new List<string>();
		public List<string> handStats = new List<string>();
		public List<string> finalResults = new List<string>();

		public int cardNumber = 0;
		public int card2Number = 0;

		public string name = "";

		public double initialBankroll;
		public double bankroll;
		public double bet;
		public double bet2 = 0;
		public double betResult = 0;
		public double bet2Result = 0;
		public double betTotal = 0;
		public double betWon = 0;
		public double insuranceBet = 0;
		public double insuranceBetResult = 0;

		public bool gameOver = false;
		public bool turnOver = false;
		public bool firstOption = true;
		public bool first2Option = true;
		public bool secondHand = false;
		public bool isSplit = false;
		public bool insurance = false;

		public int totalNaturalBJ = 0;
		public int totalDealerNaturalBJ = 0;
		public int totalPushNaturalBJ = 0;
		public int totalInsuranceOffered = 0;
		public int totalInsuranceDeclined = 0;
		public int totalInsuranceDeclineLoss = 0;
		public int totalInsuranceBought = 0;
		public int totalInsuranceWin = 0;
		public int totalInsuranceLoss = 0;
		public int totalWin = 0;
		public int totalWin2 = 0;
		public int totalPush = 0;
		public int totalSplitOffered = 0;
		public int totalSplitDeclined = 0;
		public int totalSplitAccepted = 0;
		public int totalSplitWin = 0;
		public int totalSplitLoss = 0;
		public int totalSplitPush = 0;
		public int totalSplit2Win = 0;
		public int totalSplit2Loss = 0;
		public int totalSplit2Push = 0;
		public int totalDoubleDownOffered = 0;
		public int totalDoubleDownAccepted = 0;
		public int totalDoubleDownDeclined = 0;
		public int totalBust = 0;
		public int totalLoss = 0;
		public int totalLoss2 = 0;
		public int totalDoubleDown1Win = 0;
		public int totalDoubleDown1Loss = 0;
		public int totalDoubleDown1Push = 0;
		public int totalDoubleDown2Win = 0;
		public int totalDoubleDown2Loss = 0;
		public int totalDoubleDown2Push = 0;

		public int handNumber;
		public int total = 0;
		public int total2 = 0;
		public int doubleDownFlag = 0;
		public int doubleDown2Flag = 0;
		public int insuranceFlag = 0;
		public int handWin = 0;
		public int handLoss = 0;
		public int handPush = 0;
		public int handWin2 = 0;
		public int handLoss2 = 0;
		public int handPush2 = 0;
		public int handSplitWin = 0;
		public int handSplitLoss = 0;
		public int handSplitPush = 0;
		public int handSplit2Win = 0;
		public int handSplit2Loss = 0;
		public int handSplit2Push = 0;
		public int handDoubleDown1Win = 0;
		public int handDoubleDown1Loss = 0;
		public int handDoubleDown1Push = 0;
		public int handDoubleDown2Win = 0;
		public int handDoubleDown2Loss = 0;
		public int handDoubleDown2Push = 0;
		public int handInsuranceWin = 0;
		public int handInsuranceLoss = 0;
		public int handInsuranceDeclineLoss = 0;
		public int handDoubleDownOffered = 0;
		public int handDoubleDownAccepted = 0;
		public int handDoubleDownDeclined = 0;
		public int handSplitOffered = 0;
		public int handSplitAccepted = 0;
		public int handSplitDeclined = 0;
		public int handBust = 0;
		public int handBust2 = 0;
		public int handNaturalBJ = 0;
		public int handDealerNaturalBJ = 0;
		public int handPushNaturalBJ = 0;
		public int handInsuranceOffered = 0;
		public int handInsuranceBought = 0;
		public int handInsuranceDeclined = 0;

		public bool getInsurance = false;
		public bool allowSplitDealer = false;
		public bool allowSplit = false;
		public bool allowDD = false;
	}
}
