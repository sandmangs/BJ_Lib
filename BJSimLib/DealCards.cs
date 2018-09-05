using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BJSimLib
{
    public class DealCards : CardDecks
    {
		PlayerModel player1 = new PlayerModel();
		PlayerModel player2 = new PlayerModel();
		PlayerModel player3 = new PlayerModel();
		PlayerModel player4 = new PlayerModel();
		PlayerModel dealer = new PlayerModel();
		PlayerModel gamePlayer1 = new PlayerModel();
		List<PlayerModel> players = new List<PlayerModel>();
		List<PlayerModel> gamePlayers = new List<PlayerModel>();

		public const int dealerCardPosition = 2;
		public const int playerCardPosition = 15;
		public const int player2CardPosition = 31;
		public const int playerMessagePosition = 27;
		public const int player2MessagePosition = 43;
		public const int playerBetPosition = 35;
		public const int headerBetPosition = 120;
		public const int headerBankPosition = 80;

		int x = 0;
		int y = 0;

		bool showDealCard = false;
		string nextMove = "";
		int runTimes = 0;
		double initialBankTotal = 0;
		double betAmount = 0;
		bool playGame = false;
		bool hitOnSoft17 = false;
		bool offerInsurance = false;
		bool allowSplitFlag = false;
		bool allowDoubleDown = false;
		bool ins11OrLessFlag = false;
		bool ins12To16Flag = false;
		bool ins17Flag = false;
		bool ins18Flag = false;
		bool ins19Flag = false;
		bool ins20Flag = false;
		bool splitDealer2To6Flag = false;
		bool splitDealer7Flag = false;
		bool splitDealer8Flag = false;
		bool splitDealer9Flag = false;
		bool splitDealer10Flag = false;
		bool splitDealerAceFlag = false;
		bool splitPlayer2To6Flag = false;
		bool splitPlayer7Flag = false;
		bool splitPlayer8Flag = false;
		bool splitPlayer9Flag = false;
		bool splitPlayer10Flag = false;
		bool splitPlayerAceFlag = false;
		bool dDDealer2To6Flag = false;
		bool dDDealer7Flag = false;
		bool dDDealer8Flag = false;
		bool dDDealer9Flag = false;
		bool dDDealer10Flag = false;
		bool dDDealerAceFlag = false;
		bool dDPlayer9Flag = false;
		bool dDPlayer10Flag = false;
		bool dDPlayerAceFlag = false;
		int hitOnHard2To6 = 0;
		int hitOnHard7 = 0;
		int hitOnHard8 = 0;
		int hitOnHard9 = 0;
		int hitOnHard10 = 0;
		int hitOnHardAce = 0;
		int hitOnSoft2To6 = 0;
		int hitOnSoft7 = 0;
		int hitOnSoft8 = 0;
		int hitOnSoft9 = 0;
		int hitOnSoft10 = 0;
		int hitOnSoftAce = 0;

		public DealCards()
		{
		}

		public void Intro(bool plyGame)
		{
			players.Add(gamePlayer1);
			bool acceptEntry = false;

			playGame = plyGame;
			Console.BackgroundColor = ConsoleColor.White;
			Console.Clear();
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("*********************************************");
			Console.WriteLine("***** Welcome to My Blackjack Model App *****");
			Console.WriteLine("*****            Greg Sanders           *****");
			Console.WriteLine("*********************************************");
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine();
			Console.WriteLine("Model to determine best actions after initial deal!");
			Console.WriteLine();
			Console.Write("What is your name?  ");
			gamePlayer1.name = Console.ReadLine();
			if (gamePlayer1.name.Length > 10)
			{
				gamePlayer1.name = gamePlayer1.name.Remove(10);
			}
			Console.WriteLine();
			Console.WriteLine("Welcome to the game, {0}!  Please enter the following to begin playing!", gamePlayer1.name);
			Console.WriteLine();
			do
			{
				Console.Write("Starting Bankroll ($100 - $1,000,000):  ");
				gamePlayer1.bankroll = double.Parse(Console.ReadLine());
				gamePlayer1.initialBankroll = gamePlayer1.bankroll;
				if (gamePlayer1.bankroll < 100 || gamePlayer1.bankroll > 1000000)
				{
					Console.WriteLine();
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Invalid number, please re-enter!");
					Console.ForegroundColor = ConsoleColor.Black;
					Console.SetCursorPosition(0, 11);
					ClearCurrentConsoleLine();
				}
				else
				{
					acceptEntry = true;
					Console.SetCursorPosition(0, 13);
					ClearCurrentConsoleLine();
					Console.SetCursorPosition(0, 12);
				}
			} while (!acceptEntry);
			Console.WriteLine();
			acceptEntry = false;
			do
			{
				Console.Write("Bet Size ($5 - $9,999):  ");
				betAmount = double.Parse(Console.ReadLine());
				if (betAmount < 5 || betAmount > 9999)
				{
					Console.WriteLine();
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Invalid number, please re-enter!");
					Console.ForegroundColor = ConsoleColor.Black;
					Console.SetCursorPosition(0, 13);
					ClearCurrentConsoleLine();
				}
				else
				{
					acceptEntry = true;
				}
			} while (!acceptEntry);

			gamePlayer1.bet = betAmount;
			SetUpDeck(6);
		}

		public void GameDeal()
		{
			foreach (PlayerModel p in players)
			{
				initialBankTotal = p.bankroll;
				p.handStats.Add("H#,W/L,Dlr,Ply,Ply2,DlrC,PlyrC,Plyr2C,pW,PL,pPsh,p2W,p2L,p2Push,OffDD,AccDD,DecDD,PWDD,PLDD,PshDD,P2WDD,P2LDD,Psh2DD,OffSpl,AccSpl,DecSpl,SplPW,SplPL,SplPush,SplP2W,SplP2L,Spl2Psh,PBst,P2Bst,DBst,PNBJ,DNBJ,PshNBJ,OffIns,DecIns,BIns,WInsBet,LInsBet,DInsLs,F2T,D1C");
				p.startingHandTotals.Add("Start Hand,Hands,W/L,Ws,Ls,Ps,Hit Ws,Hit Ls,Hit Ps,Hit Busts,Stand Ws,Stand Ls,Stand Ps");
			}
			InitializeConsoleDisplay();
			if (nextCardIndex > cardMark)
			{
				SetUpDeck(decks);
			}
			ResetGame();
			foreach (PlayerModel p in players)
			{
				SetUpBankroll(p);
				GameBankUpdate(p);
				InitialDeal();
				DisplayCards(p);
				GamePlayCards(p);
			}
			DealerTurn();
			GetStats();	
		}

		private static void ClearCurrentConsoleLine()
		{
			int currentLineCursor = Console.CursorTop;
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, currentLineCursor);
		}

		private void GamePlayCards(PlayerModel p)
		{
			//char playerMove = ' '; // keypress from player
			if (!p.gameOver)
			{
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine("Play Options");
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				ClearCurrentConsoleLine();
				Console.Write("(H)it   (S)tay   ");
				if (p.firstOption && (int)dealer.hand[0].MyValue == 11 && !p.insurance)
				{
					Console.Write("(I)nsurance   ");
				}
				if (p.firstOption && (p.total == 9 || p.total == 10 || p.total == 11))
				{
					Console.Write("(D)ouble Down   ");
				}
				if (p.firstOption && p.hand[0].MyFaceValue == p.hand[1].MyFaceValue)
				{
					Console.Write("s(P)lit   ");
				}
				Console.WriteLine();
			}
			while (!p.gameOver)
			{
				ConsoleKeyInfo playerMove = Console.ReadKey(true);
				if ((playerMove.KeyChar == 'I' || playerMove.KeyChar == 'i') && (int)dealer.hand[0].MyValue == 11)
				{
					Console.SetCursorPosition(x, y);
					ClearCurrentConsoleLine();
					Console.SetCursorPosition(x, y + 1);
					ClearCurrentConsoleLine();
					InsuranceOption(p);
				}
				else if (p.firstOption && (int)dealer.hand[0].MyValue == 11 && dealer.total == 21)
				{
					Console.SetCursorPosition(0, y);
					ClearCurrentConsoleLine();
					Console.SetCursorPosition(0, y + 1);
					ClearCurrentConsoleLine();
					Console.SetCursorPosition(0, y);
					Console.ForegroundColor = ConsoleColor.DarkRed;
					Console.WriteLine("Dealer has a natural Blackjack!  You didn't buy insurance and lose the hand!");
					p.betResult = -p.bet;
					Console.ForegroundColor = ConsoleColor.Red;
					Console.SetCursorPosition(playerBetPosition, playerCardPosition - 1);
					Console.Write("Bet Won: ${0}", p.betResult);
					Console.Beep();
					p.gameOver = true;
					ShowDealerCard();
					HandTotal(p);
				}
				else if (playerMove.KeyChar == 'H' || playerMove.KeyChar == 'h')
				{
					HitMe(p);
					if (p.total > 21 && !p.secondHand)
					{
						PlayerBusted(p);
					}
					else if (p.total2 > 21 && p.secondHand)
					{
						PlayerBusted(p);
					}
				}

				else if ((playerMove.KeyChar == 'D' || playerMove.KeyChar == 'd') && ((p.firstOption && !p.secondHand) || (p.first2Option && p.secondHand)) && (p.total == 9 || p.total == 10 || p.total == 11))
				{
					DoubleDown(p);
					if (!p.gameOver && !p.isSplit || p.secondHand)
					{
						DealerTurn();
					}
					else if (p.isSplit && !p.secondHand)
					{
						p.secondHand = true;
					}
				}
				else if (playerMove.KeyChar == 'S' || playerMove.KeyChar == 's')
				{

					if (!p.isSplit || p.secondHand)
					{
						Console.SetCursorPosition(x, y);
						ClearCurrentConsoleLine();
						Console.SetCursorPosition(x, y + 1);
						ClearCurrentConsoleLine();
						Console.SetCursorPosition(x, y + 2);
						ClearCurrentConsoleLine();
						DealerTurn();
					}
					else if (p.isSplit && !p.secondHand)
					{
						Console.SetCursorPosition(x, y);
						ClearCurrentConsoleLine();
						Console.SetCursorPosition(x, y + 1);
						ClearCurrentConsoleLine();
						Console.SetCursorPosition(x, y + 2);
						ClearCurrentConsoleLine();
						// y = player2MessagePosition + 4;
						p.secondHand = true;
					}
				}
				else if ((playerMove.KeyChar == 'P' || playerMove.KeyChar == 'p') && p.firstOption && p.hand[0].MyFaceValue == p.hand[1].MyFaceValue)
				{
					Console.SetCursorPosition(0, playerMessagePosition + 2);
					ClearCurrentConsoleLine();
					p.isSplit = true;
					InitializeSplitHands(p);
				}
				else
				{
					Console.SetCursorPosition(x, y);
					ClearCurrentConsoleLine();
					Console.SetCursorPosition(x, y + 1);
					ClearCurrentConsoleLine();
					Console.SetCursorPosition(x, y + 2);
					ClearCurrentConsoleLine();
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Invalid key pressed!");
				}
				if (!p.gameOver)
				{
					if (!p.secondHand)
					{
						Console.ForegroundColor = ConsoleColor.Black;
						Console.SetCursorPosition(0, y);
						Console.WriteLine("Play Options");
						Console.SetCursorPosition(x, y + 1);
						ClearCurrentConsoleLine();
						Console.ForegroundColor = ConsoleColor.DarkCyan;
						Console.Write("(H)it   (S)tay   ");
						if (p.firstOption && (int)dealer.hand[0].MyValue == 11 && !p.insurance)
						{
							Console.Write("(I)nsurance   ");
						}
						if (p.firstOption && (p.total == 9 || p.total == 10 || p.total == 11))
						{
							Console.Write("(D)ouble Down   ");
						}
						if (p.firstOption && p.hand[0].MyValue == p.hand[1].MyValue)
						{
							Console.Write("s(P)lit   ");
						}
						Console.WriteLine();
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Black;
						y = player2MessagePosition;
						Console.SetCursorPosition(0, y);
						Console.WriteLine("Play Options");
						Console.SetCursorPosition(x, y + 1);
						ClearCurrentConsoleLine();
						Console.ForegroundColor = ConsoleColor.DarkCyan;
						Console.Write("(H)it   (S)tay   ");
						if (p.firstOption && (int)dealer.hand[0].MyValue == 11 && !p.insurance)
						{
							Console.Write("(I)nsurance   ");
						}
						if (((p.firstOption && !p.secondHand) || (p.first2Option && p.secondHand)) && (p.total == 9 || p.total == 10 || p.total == 11))
						{
							Console.Write("(D)ouble Down   ");
						}
						if (p.firstOption && p.hand[0].MyValue == p.hand[1].MyValue)
						{
							Console.Write("s(P)lit   ");
						}
						Console.WriteLine();
						ClearCurrentConsoleLine();
					}
				}
				else
				{
					GameBankUpdate(p);
				}
			}
		}

		private void DisplayCards(PlayerModel p)
		{
			x = 0;
			y = dealerCardPosition;

			// Display dealer hand
			Console.SetCursorPosition(0, y - 1);
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine("DEALER'S HAND");
			for (int i = 0; i < 2; i++)
			{
				if (i == 1) // Dealer down card
				{
					DrawCards.DrawCardOutlineBack(x, y);  // back of card
				}
				else
				{
					DrawCards.DrawCardOutline(x, y);
					DrawCards.DrawCardSuitValue(dealer.hand[i], x, y);
				}
				x++; // Move cursor to the right for next card
			}

			// Display player bet
			y = playerCardPosition;
			x = 0;
			Console.SetCursorPosition(playerBetPosition, y - 1);
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.Write("Bet: ${0}", p.bet);

			// Display player hand
			Console.SetCursorPosition(x, y - 1);
			Console.ForegroundColor = ConsoleColor.DarkMagenta;
			Console.WriteLine("PLAYER'S HAND");
			for (int i = 0; i < 2; i++)
			{
				DrawCards.DrawCardOutline(x, y);
				DrawCards.DrawCardSuitValue(p.hand[i], x, y);
				x++;
			}
			y = playerMessagePosition; // move row below player cards
			x = 0;
			Console.SetCursorPosition(x, y);

			p.cardNumber = 1;
			dealer.cardNumber = 1;
			HandTotal(p);  // Display the player hand total and dealer 'unknown' label
		}

		private void GameInitialDeal(PlayerModel p)
		{
			if (p.total == 21 && dealer.total != 21)  // Player wins 1.5 times the bet with a natural BJ and dealer doesn't
			{
				Console.SetCursorPosition(0, y);
				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine("BLACKJACK!!!  Player WINS!!!!");
				Console.SetCursorPosition(playerBetPosition, playerCardPosition - 1);
				Console.Write("Bet Won: ${0}", p.betResult);
				for (int i = 0; i < 3; i++)
				{
					Console.Beep();
				}
				GameBankUpdate(p);
				ShowDealerCard();
				Console.SetCursorPosition(0, y + 2);
			}
			else if (p.total == 21 && dealer.total == 21)  // Player pushes with natural BJ if dealer also had natural BJ
			{
				Console.SetCursorPosition(0, y);
				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine("Both the Player and Dealer have BLACKJACK!!!  Tie game, Player takes back his chips!!!!");
				Console.ForegroundColor = ConsoleColor.DarkMagenta;
				Console.SetCursorPosition(playerBetPosition, playerCardPosition - 1);
				Console.Write("Bet Won: ${0}", p.betResult);
				Console.Beep();
				GameBankUpdate(p);
				ShowDealerCard();
				Console.SetCursorPosition(0, y + 2);
			}
		}

		public void GameBankUpdate(PlayerModel p)  // Bankroll display and management
		{
			Console.SetCursorPosition(headerBetPosition, 0);
			if (p.cardNumber == 0)  // First call per hand, set up bet / bankroll header
			{
				Console.ForegroundColor = ConsoleColor.DarkBlue;
				Console.Write("Bet: ${0}", p.bet);
				Console.SetCursorPosition(headerBankPosition, 0);
				Console.Write("Bankroll: ${0}", p.bankroll);
				Console.SetCursorPosition(x, y+1);
			}
			else if (!p.gameOver)  //  Update bet amounts based on double / insurance / split
			{
				Console.ForegroundColor = ConsoleColor.DarkBlue;
				Console.Write("Bet: ${0}", p.bet + p.bet2 + p.insuranceBet);
				Console.SetCursorPosition(x, y+1);
			}
			else  //  Game over bankroll calculations and display
			{
				//p.betWon = p.betResult + p.bet2Result + p.insuranceBetResult;
				if (p.betWon > 0)
				{
					Console.ForegroundColor = ConsoleColor.DarkGreen;
				}
				else if (p.betWon == 0)
				{
					Console.ForegroundColor = ConsoleColor.DarkMagenta;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				Console.Write("Bet Won: ${0}", p.betWon);
				Console.SetCursorPosition(headerBankPosition, 0);
				Console.ForegroundColor = ConsoleColor.DarkBlue;
				p.bankroll += p.betWon;
				Console.Write("Bankroll: ${0}", p.bankroll);
				Console.SetCursorPosition(0, y + 3);
			}
		}

		private void ShowDealerCard()
		{
			int temp = y;
			x = 0; //x position of the cursor. We move it left and right
			y = dealerCardPosition;//y position of the cursor, we move up and down

			//display dealer hand
			for (int i = 0; i < 2; i++)
			{
				DrawCards.DrawCardOutline(x, y);
				DrawCards.DrawCardSuitValue(dealer.hand[i], x, y);
				x++;//move to the right
				showDealCard = true; // TODO move outside of for loop
			}
			y = temp;
		}

		public void SetHandVariables(bool hitSoft17, bool offerIns, bool allowSplt, bool allowDD, int hOnHard2To6, int hOnHard7, int hOnHard8, int hOnHard9, int hOnHard10, int hOnHardAce, int hOnSoft2To6, int hOnSoft7, int hOnSoft8, int hOnSoft9, int hOnSoft10, int hOnSoftAce)
		{
			hitOnSoft17 = hitSoft17;
			offerInsurance = offerIns;
			allowSplitFlag = allowSplt;
			allowDoubleDown = allowDD;
			hitOnHard2To6 = hOnHard2To6;
			hitOnHard7 = hOnHard7;
			hitOnHard8 = hOnHard8;
			hitOnHard9 = hOnHard9;
			hitOnHard10 = hOnHard10;
			hitOnHardAce = hOnHardAce;
			hitOnSoft2To6 = hOnSoft2To6;
			hitOnSoft7 = hOnSoft7;
			hitOnSoft8 = hOnSoft8;
			hitOnSoft9 = hOnSoft9;
			hitOnSoft10 = hOnSoft10;
			hitOnSoftAce = hOnSoftAce;
		}

		public void SetInsuranceVariables(bool ins11OrLess, bool ins12To16, bool ins17, bool ins18, bool ins19, bool ins20)
		{
			ins11OrLessFlag = ins11OrLess;
			ins12To16Flag = ins12To16;
			ins17Flag = ins17;
			ins18Flag = ins18;
			ins19Flag = ins19;
			ins20Flag = ins20;
		}

		public void SetDDVariables(bool dDDealer2To6, bool dDDealer7, bool dDDealer8, bool dDDealer9, bool dDDealer10, bool dDDealerAce, bool dDPlayer9, bool dDPlayer10, bool dDPlayerAce)
		{
			dDDealer2To6Flag = dDDealer2To6;
			dDDealer7Flag = dDDealer7;
			dDDealer8Flag = dDDealer8;
			dDDealer9Flag = dDDealer9;
			dDDealer10Flag = dDDealer10;
			dDDealerAceFlag = dDDealerAce;
			dDPlayer9Flag = dDPlayer9;
			dDPlayer10Flag = dDPlayer10;
			dDPlayerAceFlag = dDPlayerAce;
		}

		public void SetSplitVariables(bool splitDealer2To6, bool splitDealer7, bool splitDealer8, bool splitDealer9, bool splitDealer10, bool splitDealerAce, bool splitPlayer2To6, bool splitPlayer7, bool splitPlayer8, bool splitPlayer9, bool splitPlayer10, bool splitPlayerAce)
		{
			splitDealer2To6Flag = splitDealer2To6;
			splitDealer7Flag = splitDealer7;
			splitDealer8Flag = splitDealer8;
			splitDealer9Flag = splitDealer9;
			splitDealer10Flag = splitDealer10;
			splitDealerAceFlag = splitDealerAce;
			splitPlayer2To6Flag = splitPlayer2To6;
			splitPlayer7Flag = splitPlayer7;
			splitPlayer8Flag = splitPlayer8;
			splitPlayer9Flag = splitPlayer9;
			splitPlayer10Flag = splitPlayer10;
			splitPlayerAceFlag = splitPlayerAce;
		}

		private void AllowDD(PlayerModel p, int handTotal)
		{
			p.allowDD = false;
			switch (handTotal)
			{
				case 9:
					if (dDPlayer9Flag)
					{
						switch ((int)dealer.hand[0].MyValue)
						{
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
								if (dDDealer2To6Flag)
								{
									p.allowDD = true;
								}
								break;
							case 7:
								if (dDDealer7Flag)
								{
									p.allowDD = true;
								}
								break;
							case 8:
								if (dDDealer8Flag)
								{
									p.allowDD = true;
								}
								break;
							case 9:
								if (dDDealer9Flag)
								{
									p.allowDD = true;
								}
								break;
							case 10:
								if (dDDealer10Flag)
								{
									p.allowDD = true;
								}
								break;
							case 11:
								if (dDDealerAceFlag)
								{
									p.allowDD = true;
								}
								break;
						}
					}
					break;
				case 10:
					if (dDPlayer10Flag)
					{
						switch ((int)dealer.hand[0].MyValue)
						{
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
								if (dDDealer2To6Flag)
								{
									p.allowDD = true;
								}
								break;
							case 7:
								if (dDDealer7Flag)
								{
									p.allowDD = true;
								}
								break;
							case 8:
								if (dDDealer8Flag)
								{
									p.allowDD = true;
								}
								break;
							case 9:
								if (dDDealer9Flag)
								{
									p.allowDD = true;
								}
								break;
							case 10:
								if (dDDealer10Flag)
								{
									p.allowDD = true;
								}
								break;
							case 11:
								if (dDDealerAceFlag)
								{
									p.allowDD = true;
								}
								break;
						}
					}
					break;
				case 11:
					if (dDPlayerAceFlag)
					{
						switch ((int)dealer.hand[0].MyValue)
						{
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
								if (dDDealer2To6Flag)
								{
									p.allowDD = true;
								}
								break;
							case 7:
								if (dDDealer7Flag)
								{
									p.allowDD = true;
								}
								break;
							case 8:
								if (dDDealer8Flag)
								{
									p.allowDD = true;
								}
								break;
							case 9:
								if (dDDealer9Flag)
								{
									p.allowDD = true;
								}
								break;
							case 10:
								if (dDDealer10Flag)
								{
									p.allowDD = true;
								}
								break;
							case 11:
								if (dDDealerAceFlag)
								{
									p.allowDD = true;
								}
								break;
						}
					}
					break;
			}
			if (!p.allowDD)
			{
				p.totalDoubleDownDeclined++;
				p.handDoubleDownDeclined = 1;
			}
		}

		private void AllowSplit(PlayerModel player)
		{
			switch ((int)player.hand[0].MyValue)
			{
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
					if (splitPlayer2To6Flag)
					{
						switch ((int)dealer.hand[0].MyValue)
						{
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
								if (splitDealer2To6Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 7:
								if (splitDealer7Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 8:
								if (splitDealer8Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 9:
								if (splitDealer9Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 10:
								if (splitDealer10Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 11:
								if (splitDealerAceFlag)
								{
									player.allowSplit = true;
								}
								break;
						}
					}
					break;
				case 7:
					if (splitPlayer7Flag)
					{
						switch ((int)dealer.hand[0].MyValue)
						{
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
								if (splitDealer2To6Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 7:
								if (splitDealer7Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 8:
								if (splitDealer8Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 9:
								if (splitDealer9Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 10:
								if (splitDealer10Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 11:
								if (splitDealerAceFlag)
								{
									player.allowSplit = true;
								}
								break;
						}
					}
					break;
				case 8:
					if (splitPlayer8Flag)
					{
						switch ((int)dealer.hand[0].MyValue)
						{
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
								if (splitDealer2To6Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 7:
								if (splitDealer7Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 8:
								if (splitDealer8Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 9:
								if (splitDealer9Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 10:
								if (splitDealer10Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 11:
								if (splitDealerAceFlag)
								{
									player.allowSplit = true;
								}
								break;
						}
					}
					break;
				case 9:
					if (splitPlayer9Flag)
					{
						switch ((int)dealer.hand[0].MyValue)
						{
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
								if (splitDealer2To6Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 7:
								if (splitDealer7Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 8:
								if (splitDealer8Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 9:
								if (splitDealer9Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 10:
								if (splitDealer10Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 11:
								if (splitDealerAceFlag)
								{
									player.allowSplit = true;
								}
								break;
						}
					}
					break;
				case 10:
					if (splitPlayer10Flag)
					{
						switch ((int)dealer.hand[0].MyValue)
						{
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
								if (splitDealer2To6Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 7:
								if (splitDealer7Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 8:
								if (splitDealer8Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 9:
								if (splitDealer9Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 10:
								if (splitDealer10Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 11:
								if (splitDealerAceFlag)
								{
									player.allowSplit = true;
								}
								break;
						}
					}
					break;
				case 11:
					if (splitPlayerAceFlag)
					{
						switch ((int)dealer.hand[0].MyValue)
						{
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
								if (splitDealer2To6Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 7:
								if (splitDealer7Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 8:
								if (splitDealer8Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 9:
								if (splitDealer9Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 10:
								if (splitDealer10Flag)
								{
									player.allowSplit = true;
								}
								break;
							case 11:
								if (splitDealerAceFlag)
								{
									player.allowSplit = true;
								}
								break;
						}
					}
					break;
			}
			if (player.allowSplit == false)
			{
				player.totalSplitDeclined++;
				player.handSplitDeclined = 1;
			}
		}

		private void GetInsurance(PlayerModel player)
		{

			if (player.total <= 11 && ins11OrLessFlag)
			{
				player.getInsurance = true;
			}
			else if (player.total >= 12 && player.total <= 16 && ins12To16Flag)
			{
				player.getInsurance = true;
			}
			else if (player.total == 17 && ins17Flag)
			{
				player.getInsurance = true;
			}
			else if (player.total == 18 && ins18Flag)
			{
				player.getInsurance = true;
			}
			else if (player.total == 19 && ins19Flag)
			{
				player.getInsurance = true;
			}
			else if (player.total == 20 && ins20Flag)
			{
				player.getInsurance = true;
			}
		}

		public void Deal(int plyrBankroll, int plyrBet, int rnTimes)
		{
			players.Add(player1);
			players.Add(player2);
			players.Add(player3);
			players.Add(player4);
			runTimes = rnTimes;
			InitializeConsoleDisplay();
			foreach (PlayerModel p in players)
			{
				p.bankroll = plyrBankroll;
				initialBankTotal = p.bankroll;
				betAmount = plyrBet;
				p.handStats.Add("H#,W/L,Dlr,Ply,Ply2,DlrC,PlyrC,Plyr2C,pW,PL,pPsh,p2W,p2L,p2Push,OffDD,AccDD,DecDD,PWDD,PLDD,PshDD,P2WDD,P2LDD,Psh2DD,OffSpl,AccSpl,DecSpl,SplPW,SplPL,SplPush,SplP2W,SplP2L,Spl2Psh,PBst,P2Bst,DBst,PNBJ,DNBJ,PshNBJ,OffIns,DecIns,BIns,WInsBet,LInsBet,DInsLs,F2T,D1C");
				p.startingHandTotals.Add("Start Hand,Hands,W/L,Ws,Ls,Ps,Hit Ws,Hit Ls,Hit Ps,Hit Busts,Stand Ws,Stand Ls,Stand Ps"); 
			}
			for (int i = 0; i < runTimes; i++)
			{
				if (nextCardIndex > cardMark)
				{
					SetUpDeck(decks);
				}
				ResetGame();
				foreach (PlayerModel p in players)
				{
					SetUpBankroll(p);
				}
				InitialDeal();
				foreach (PlayerModel p in players)
				{
					if (offerInsurance && (int)dealer.hand[0].MyValue == 11)
					{
						GetInsurance(p);
					}
					if ((int)p.hand[0].MyFaceValue == (int)p.hand[1].MyFaceValue && allowSplitFlag && dealer.total != 21)
					{
						p.totalSplitOffered++;
						p.handSplitOffered = 1;
						AllowSplit(p);
					}

					if ((p.total == 9 || p.total == 10 || p.total == 11) && allowDoubleDown && dealer.total != 21)
					{
						p.totalDoubleDownOffered++;
						p.handDoubleDownOffered = 1;
						AllowDD(p, p.total);
					}
					PlayCards(p);
				}
				DealerTurn();
				DisplayBank();
				GetStats();
			}
			DisplayStats();
			WriteToFile();
			Console.ReadLine();
		}

		private void PlayCards(PlayerModel p)
		{
			while (!p.turnOver)
			{
				SimulatorLogic(p);
				if (!p.insurance && offerInsurance && (int)dealer.hand[0].MyValue == 11 && p.getInsurance)
				{
					InsuranceOption(p);
				}
				else if (p.firstOption && (int)dealer.hand[0].MyValue == 11 && dealer.total == 21)
				{
					p.betResult = 0;
					p.totalDealerNaturalBJ++;
					p.handDealerNaturalBJ = 1;
					p.totalInsuranceDeclineLoss++;
					p.handInsuranceDeclineLoss = 1;
					p.totalLoss++;
					p.handLoss = 1;
					p.turnOver = true;
					p.gameOver = true;
					BankUpdate(p);
				}
				else if (nextMove == "Dealer21")
				{
					p.betResult = 0;
					p.totalDealerNaturalBJ++;
					p.handDealerNaturalBJ = 1;
					p.totalLoss++;
					p.handLoss = 1;
					p.turnOver = true;
					p.gameOver = true;
					BankUpdate(p);
				}
				else if (nextMove == "HitMe")
				{
					HitMe(p);
					if (p.total > 21 && !p.secondHand)
					{
						PlayerBusted(p);
					}
					else if (p.total2 > 21 && p.secondHand)
					{
						PlayerBusted(p);
					}
				}
				else if (nextMove == "DoubleDown")
				{
					p.totalDoubleDownAccepted++;
					p.handDoubleDownAccepted = 1;
					if (!p.secondHand)
					{
						p.doubleDownFlag = 1;
					}
					else
					{
						p.doubleDown2Flag = 1;
					}
					DoubleDown(p);
					if (!p.gameOver && (!p.isSplit || p.secondHand))
					{
						p.turnOver = true;
					}
					else if (p.isSplit && !p.secondHand)
					{
						p.secondHand = true;
					}
				}
				else if (nextMove == "Stand")
				{
					if (!p.isSplit || p.secondHand)
					{
						p.turnOver = true;
					}
					else if (p.isSplit && !p.secondHand)
					{
						p.secondHand = true;
					}
				}
				else if (nextMove == "Split")
				{
					p.totalSplitAccepted++;
					p.handSplitAccepted = 1;
					p.isSplit = true;
					InitializeSplitHands(p);
				}
				else
				{
					Console.SetCursorPosition(5, 1);
					Console.WriteLine("Invalid key pressed!");
					Console.ReadLine();
					p.turnOver = true;
					p.gameOver = true;
					BankUpdate(p);
				}
				nextMove = "";
			}
		}

		private void SimulatorLogic(PlayerModel p)
		{
			if (p.firstOption && (int)dealer.hand[0].MyValue != 11 && dealer.total == 21)
			{
				nextMove = "Dealer21";
			}
			if (nextMove == "" && p.firstOption && p.allowSplit)
			{
				nextMove = "Split";
			}
			if (nextMove == "" && p.firstOption && p.allowDD)
			{
				nextMove = "DoubleDown";
			}
			else if (nextMove == "" && p.secondHand && p.first2Option && (p.total2 >= 9 && p.total2 <= 11) && allowDoubleDown)
			{
				p.totalDoubleDownOffered++;
				p.handDoubleDownOffered = 1;
				AllowDD(p, p.total2);
				if (p.allowDD)
				{
					nextMove = "DoubleDown";
				}
			}
			if (nextMove == "")
			{
				int handTotal = 0;
				bool softAce = false;
				if (!p.secondHand)
				{
					handTotal = p.total;
					softAce = IfSoftAce(p.hand);
				}
				else
				{
					handTotal = p.total2;
					softAce = IfSoftAce(p.hand2);
				}
				switch ((int)dealer.hand[0].MyValue)
				{
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
						if (softAce)
						{
							if (hitOnSoft2To6 > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						else
						{
							if (hitOnHard2To6 > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						break;
					case 7:
						if (softAce)
						{
							if (hitOnSoft7 > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						else
						{
							if (hitOnHard7 > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						break;
					case 8:
						if (softAce)
						{
							if (hitOnSoft8 > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						else
						{
							if (hitOnHard8 > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						break;
					case 9:
						if (softAce)
						{
							if (hitOnSoft9 > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						else
						{
							if (hitOnHard9 > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						break;
					case 10:
						if (softAce)
						{
							if (hitOnSoft10 > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						else
						{
							if (hitOnHard10 > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						break;
					case 11:
						if (softAce)
						{
							if (hitOnSoftAce > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						else
						{
							if (hitOnHardAce > handTotal)
							{
								nextMove = "HitMe";
							}
							else
							{
								nextMove = "Stand";
							}
						}
						break;
				}
			}
		}

		public void WriteToFile()
		{
			int playnum = 1;
			double totalResult = 0;
			foreach (PlayerModel p in players)
			{
				string path = @"c:\temp\player" + playnum + "hands.csv";
				string path2 = @"c:\temp\player" + playnum + "hands.txt";

				File.WriteAllLines(path, p.handStats);

				p.finalResults.Add("    Total Number of Hands:  " + p.handNumber);
				p.finalResults.Add("Player Natural Blackjacks:  " + p.totalNaturalBJ);
				p.finalResults.Add("Dealer Natural Blackjacks:  " + p.totalDealerNaturalBJ);
				p.finalResults.Add("  Push Natural Blackjacks:  " + p.totalPushNaturalBJ);
				p.finalResults.Add("        Offered Insurance:  " + p.totalInsuranceOffered);
				p.finalResults.Add("       Declined Insurance:  " + p.totalInsuranceDeclined);
				p.finalResults.Add("         Bought Insurance:  " + p.totalInsuranceBought);
				p.finalResults.Add("        Won Insurance Bet:  " + p.totalInsuranceWin);
				p.finalResults.Add("       Lost Insurance Bet:  " + p.totalInsuranceLoss);
				p.finalResults.Add("Declined Insurance Losses:  " + p.totalInsuranceDeclineLoss);
				p.finalResults.Add("      Offered Double Down:  " + p.totalDoubleDownOffered);
				p.finalResults.Add("     Accepted Double Down:  " + p.totalDoubleDownAccepted);
				p.finalResults.Add("     Declined Double Down:  " + p.totalDoubleDownDeclined);
				p.finalResults.Add("     Player 1 DD Hand Win:  " + p.totalDoubleDown1Win);
				p.finalResults.Add("    Player 1 DD Hand Loss:  " + p.totalDoubleDown1Loss);
				p.finalResults.Add("     Player 2 DD Hand Win:  " + p.totalDoubleDown2Win);
				p.finalResults.Add("    Player 2 DD Hand Loss:  " + p.totalDoubleDown2Loss);
				p.finalResults.Add("      Offered Split Cards:  " + p.totalSplitOffered);
				p.finalResults.Add("       Accept Split Cards:  " + p.totalSplitAccepted);
				p.finalResults.Add("     Declined Split Cards:  " + p.totalSplitDeclined);
				p.finalResults.Add("               Player Win:  " + p.totalWin);
				p.finalResults.Add("              Player Loss:  " + p.totalLoss);
				p.finalResults.Add("             Pushed Hands:  " + p.totalPush);
				p.finalResults.Add("      Player 2nd Hand Win:  " + p.totalWin2);
				p.finalResults.Add("     Player 2nd Hand Loss:  " + p.totalLoss2);

				File.WriteAllLines(path2, p.finalResults);
				playnum++;
			}
			Console.SetCursorPosition(20, 42);
			Console.WriteLine("Press a key to continue");
			Console.ReadLine();
			Console.Clear();
			Console.SetCursorPosition(0, 2);
			for (int i = 4; i < 23; i++)
			{
				LoadHandsFile(i);
			}
			foreach (PlayerModel p in players)
			{
				p.startingHandTotals.Add($"Total Betx Won / Loss,,,{p.bankroll - initialBankTotal},,,,,,,,,");
				totalResult += p.bankroll - initialBankTotal;
			}
			Console.WriteLine();
			Console.WriteLine("Total Bets Won / Loss                 {0,28:c2}", totalResult);
			Console.WriteLine();
			for (int i = 2; i < 12; i++)
			{
				LoadDealerHandFile(i);
			}
			totalResult = 0;
			foreach (PlayerModel p in players)
			{
				p.startingHandTotals.Add($"Total Betx Won / Loss,,,{(p.bankroll - initialBankTotal)},,,,,,,,,");
				totalResult += p.bankroll - initialBankTotal;
			}
			Console.WriteLine();
			Console.WriteLine("Total Bets Won / Loss                {0,28:c2}", totalResult);
			Console.WriteLine();
			playnum = 1;
			foreach (PlayerModel p in players)
			{
				string path4 = @"c:\temp\player" + playnum + "startingHandResults.csv";
				File.WriteAllLines(path4, p.startingHandTotals);
				playnum++;
			}
		}

		private void LoadDealerHandFile(int cardValue)
		{
			int totalHandsTotal = 0;
			double totalBetTotal = 0;
			int totalWins = 0;
			int totalLosses = 0;
			int totalPushes = 0;
			int totalWinsHit = 0;
			int totalLossesHit = 0;
			int totalPushesHit = 0;
			int totalHitBusts = 0;
			int totalWinsStand = 0;
			int totalLossesStand = 0;
			int totalPushesStand = 0;
			int playnum = 1;
			foreach (PlayerModel p in players)
			{
				string[] lines = File.ReadAllLines(@"C:\temp\player" + playnum + "hands.csv");
				lines = lines.Skip(1).ToArray();
				foreach (string line in lines)
				{
					string[] cols = line.Split(',');

					if (cardValue == int.Parse(cols[45]))
					{
						double betTotal = 0;
						int handsTotal = 0;
						int wins = 0;
						int winsHit = 0;
						int winsStand = 0;
						int hitBusts = 0;
						double hitTotal = 0;
						int losses = 0;
						int lossesHit = 0;
						int lossesStand = 0;
						double standTotal = 0;
						int pushs = 0;
						int pushsHit = 0;
						int pushsStand = 0;
						double bet = 0;
						int win = 0;
						int loss = 0;
						int push = 0;
						string card1 = "";
						bool hit = false;
						bool stand = false;
						int firstCard = 0;
						int busts = 0;
						bet = double.Parse(cols[1]);
						win = int.Parse(cols[8]);
						loss = int.Parse(cols[9]);
						push = int.Parse(cols[10]);
						busts = int.Parse(cols[32]);
						string[] card = cols[5].Split('~');
						card1 = card[0];
						firstCard = GetCardValue(card1);
						string[] playerCards = cols[6].Split('~');
						if (playerCards.Length > 2)
						{
							hit = true;
							hitTotal += bet;
							if (busts == 1)
							{
								hitBusts++;
							}
						}
						else
						{
							stand = true;
							standTotal += bet;
						}
						handsTotal++;
						betTotal += bet;
						if (win == 1)
						{
							wins++;
							if (hit)
							{
								winsHit++;
							}
							else
							{
								winsStand++;
							}
						}
						else if (loss == 1)
						{
							losses++;
							if (hit)
							{
								lossesHit++;
							}
							else
							{
								lossesStand++;
							}
						}
						else
						{
							pushs++;
							if (hit)
							{
								pushsHit++;
							}
							else
							{
								pushsStand++;
							}
						}
						
						p.startingHandTotals.Add($"{cardValue},{handsTotal},{betTotal},{wins},{losses},{pushs},{winsHit},{lossesHit},{pushsHit},{hitBusts},{winsStand},{lossesStand},{pushsStand}");
						totalHandsTotal += handsTotal;
						totalBetTotal += betTotal;
						totalWins += wins;
						totalLosses += losses;
						totalPushes += pushs;
						totalWinsHit += winsHit;
						totalLossesHit += lossesHit;
						totalPushesHit += pushsHit;
						totalHitBusts += hitBusts;
						totalWinsStand += winsStand;
						totalLossesStand += lossesStand;
						totalPushesStand += pushsStand;
					}
				}
				playnum++;
			}

			double odds = ((double)totalHandsTotal / ((double)runTimes * players.Count())) * 100;
			int currentLineCursor = Console.CursorTop;
			Console.Write(" Starting Hand: {0,2}", cardValue);
			Console.SetCursorPosition(20, currentLineCursor);
			Console.Write("Hands: {0,5:n0}", totalHandsTotal);
			Console.SetCursorPosition(34, currentLineCursor);
			Console.Write("Per: {0,6:n2} %", odds);
			Console.SetCursorPosition(49, currentLineCursor);
			Console.Write("W/L: {0,11:c0}", totalBetTotal);
			Console.SetCursorPosition(67, currentLineCursor);
			Console.Write("Ws: {0,4:n0}", totalWins);
			Console.SetCursorPosition(77, currentLineCursor);
			Console.Write("Ls: {0,4:n0}", totalLosses);
			Console.SetCursorPosition(87, currentLineCursor);
			Console.Write("Ps: {0,4:n0}", totalPushes);
			Console.SetCursorPosition(97, currentLineCursor);
			Console.Write("Hit Ws: {0,4:n0}", totalWinsHit);
			Console.SetCursorPosition(111, currentLineCursor);
			Console.Write("Hit Ls: {0,4:n0}", totalLossesHit);
			Console.SetCursorPosition(125, currentLineCursor);
			Console.Write("Hit Ps: {0,4:n0}", totalPushesHit);
			Console.SetCursorPosition(139, currentLineCursor);
			Console.Write("Hit Busts: {0,3}", totalHitBusts);
			Console.SetCursorPosition(155, currentLineCursor);
			Console.Write("Stand Ws: {0,4:n0}", totalWinsStand);
			Console.SetCursorPosition(171, currentLineCursor);
			Console.Write("Stand Ls: {0,4:n0}", totalLossesStand);
			Console.SetCursorPosition(187, currentLineCursor);
			Console.Write("Stand Ps: {0,4:n0}", totalPushesStand);
			Console.WriteLine();
		}

		private void LoadHandsFile(int cardValue)
		{
			int totalHandsTotal = 0;
			double totalBetTotal = 0;
			int totalWins = 0;
			int totalLosses = 0;
			int totalPushes = 0;
			int totalWinsHit = 0;
			int totalLossesHit = 0;
			int totalPushesHit = 0;
			int totalHitBusts = 0;
			int totalWinsStand = 0;
			int totalLossesStand = 0;
			int totalPushesStand = 0;
			int playnum = 1;
			foreach (PlayerModel p in players)
			{
				string[] lines = File.ReadAllLines(@"C:\temp\player" + playnum + "hands.csv");
				lines = lines.Skip(1).ToArray();

				foreach (string line in lines)
				{
					string[] cols = line.Split(',');
					if (int.Parse(cols[44]) == cardValue)
					{
						double bet = 0;
						int win = 0;
						int loss = 0;
						int push = 0;
						bool hit = false;
						int busts = 0;
						double betTotal = 0;
						int handsTotal = 0;
						int wins = 0;
						int winsHit = 0;
						int winsStand = 0;
						int hitBusts = 0;
						double hitTotal = 0;
						int losses = 0;
						int lossesHit = 0;
						int lossesStand = 0;
						double standTotal = 0;
						int pushs = 0;
						int pushsHit = 0;
						int pushsStand = 0;
						bet = double.Parse(cols[1]);
						win = int.Parse(cols[8]);
						loss = int.Parse(cols[9]);
						push = int.Parse(cols[10]);
						busts = int.Parse(cols[32]);
						string[] card = cols[6].Split('~');
						if (card.Length > 2)
						{
							hit = true;
							hitTotal += bet;
							if (busts == 1)
							{
								hitBusts++;
							}
						}
						else
						{
							standTotal += bet;
						}
						handsTotal++;
						betTotal += bet;
						if (win == 1)
						{
							wins++;
							if (hit)
							{
								winsHit++;
							}
							else
							{
								winsStand++;
							}
						}
						else if (loss == 1)
						{
							losses++;
							if (hit)
							{
								lossesHit++;
							}
							else
							{
								lossesStand++;
							}
						}
						else
						{
							pushs++;
							if (hit)
							{
								pushsHit++;
							}
							else
							{
								pushsStand++;
							}
						}
						p.startingHandTotals.Add($"{cardValue},{handsTotal},{betTotal},{wins},{losses},{pushs},{winsHit},{lossesHit},{pushsHit},{hitBusts},{winsStand},{lossesStand},{pushsStand}");
						totalHandsTotal += handsTotal;
						totalBetTotal += betTotal;
						totalWins += wins;
						totalLosses += losses;
						totalPushes += pushs;
						totalWinsHit += winsHit;
						totalLossesHit += lossesHit;
						totalPushesHit += pushsHit;
						totalHitBusts += hitBusts;
						totalWinsStand += winsStand;
						totalLossesStand += lossesStand;
						totalPushesStand += pushsStand;
					}
				}
				playnum++;
			}
			double odds = ((double)totalHandsTotal / ((double)runTimes*players.Count())) * 100;
			int currentLineCursor = Console.CursorTop;
			Console.Write(" Starting Hand: {0,2}", cardValue);
			Console.SetCursorPosition(20, currentLineCursor);
			Console.Write("Hands: {0,5:n0}", totalHandsTotal);
			Console.SetCursorPosition(34, currentLineCursor);
			Console.Write("Per: {0,6:n2} %", odds);
			Console.SetCursorPosition(49, currentLineCursor);
			Console.Write("W/L: {0,11:c0}", totalBetTotal);
			Console.SetCursorPosition(67, currentLineCursor);
			Console.Write("Ws: {0,4:n0}", totalWins);
			Console.SetCursorPosition(77, currentLineCursor);
			Console.Write("Ls: {0,4:n0}", totalLosses);
			Console.SetCursorPosition(87, currentLineCursor);
			Console.Write("Ps: {0,4:n0}", totalPushes);
			Console.SetCursorPosition(97, currentLineCursor);
			Console.Write("Hit Ws: {0,4:n0}", totalWinsHit);
			Console.SetCursorPosition(111, currentLineCursor);
			Console.Write("Hit Ls: {0,4:n0}", totalLossesHit);
			Console.SetCursorPosition(125, currentLineCursor);
			Console.Write("Hit Ps: {0,4:n0}", totalPushesHit);
			Console.SetCursorPosition(139, currentLineCursor);
			Console.Write("Hit Busts: {0,3}", totalHitBusts);
			Console.SetCursorPosition(155, currentLineCursor);
			Console.Write("Stand Ws: {0,4:n0}", totalWinsStand);
			Console.SetCursorPosition(171, currentLineCursor);
			Console.Write("Stand Ls: {0,4:n0}", totalLossesStand);
			Console.SetCursorPosition(187, currentLineCursor);
			Console.Write("Stand Ps: {0,4:n0}", totalPushesStand);
			Console.WriteLine();
		}

		private int GetCardValue(string card)
		{
			switch (card)
			{
				case "2":
				case "3":
				case "4":
				case "5":
				case "6":
				case "7":
				case "8":
				case "9":
					return int.Parse(card);
				case "T":
				case "J":
				case "Q":
				case "K":
					return 10;
				case "A":
					return 11;
			}
			return 0;
		}

		private string ShortenSuit(Cards[] deck)
		{
			string cards = "";

			for (int i = 0; i < deck.Length; i++)
			{
				if (deck[i] != null)
				{
					switch ((int)deck[i].MyFaceValue)
					{

						case 2:
						case 3:
						case 4:
						case 5:
						case 6:
						case 7:
						case 8:
						case 9:
							cards += (int)deck[i].MyFaceValue + "~";
							break;
						case 10:
							cards += "T~";
							break;
						case 11:
							cards += "J~";
							break;
						case 12:
							cards += "Q~";
							break;
						case 13:
							cards += "K~";
							break;
						case 14:
							cards += "A~";
							break;
					}
				}
			}
			cards = cards.Substring(0, cards.Length - 1);
			return cards;
		}

		private void GetStats()
		{
			foreach (PlayerModel p in players)
			{
				string player2Hand = "";
				string dealerHand = ShortenSuit(dealer.hand);
				string playerHand = ShortenSuit(p.hand);
				if (p.isSplit)
				{
					player2Hand = ShortenSuit(p.hand2);
					if (p.handWin == 1)
					{
						p.totalSplitWin++;
						p.handSplitWin = 1;
					}
					else if (p.handLoss == 1)
					{
						p.totalSplitLoss++;
						p.handSplitLoss = 1;
					}
					else if (p.handPush == 1)
					{
						p.totalSplitPush++;
						p.handSplitPush = 1;
					}
					if (p.handWin2 == 1)
					{
						p.totalSplit2Win++;
						p.handSplit2Win = 1;
					}
					else if (p.handLoss2 == 1)
					{
						p.totalSplit2Loss++;
						p.handSplit2Loss = 1;
					}
					else if (p.handPush2 == 1)
					{
						p.totalSplit2Push++;
						p.handSplit2Push = 1;
					}
				}
				if (p.doubleDownFlag == 1)
				{
					if (p.betResult == p.bet)
					{
						p.totalDoubleDown1Win++;
						p.handDoubleDown1Win = 1;
					}
					else if (p.betResult < p.bet)
					{
						p.totalDoubleDown1Loss++;
						p.handDoubleDown1Loss = 1;
					}
					else
					{
						p.totalDoubleDown1Push++;
						p.handDoubleDown1Push = 1;
					}
				}
				if (p.doubleDown2Flag == 1)
				{
					if (p.bet2Result > p.bet2)
					{
						p.totalDoubleDown2Win++;
						p.handDoubleDown2Win = 1;
					}
					else if (p.bet2Result < p.bet2)
					{
						p.totalDoubleDown2Loss++;
						p.handDoubleDown2Loss = 1;
					}
					else
					{
						p.totalDoubleDown2Push++;
						p.handDoubleDown2Push = 1;
					}
				}
				if ((int)dealer.hand[0].MyValue == 11 && offerInsurance && !p.insurance && p.handInsuranceOffered == 1)
				{
					p.totalInsuranceDeclined++;
					p.handInsuranceDeclined = 1;
				}
				p.handNumber++;
				int first2Total = (int)p.hand[0].MyValue + (int)p.hand[1].MyValue;
				int dealerFirstCard = (int)dealer.hand[0].MyValue;

				p.handStats.Add($"{ p.handNumber },{ p.betResult },{ dealer.total },{ p.total },{ p.total2 },{ dealerHand },{ playerHand },{ player2Hand },{ p.handWin },{ p.handLoss },{ p.handPush }, {p.handWin2 },{ p.handLoss2 },{ p.handPush2 },{ p.handDoubleDownOffered },{ p.handDoubleDownAccepted },{ p.handDoubleDownDeclined },{ p.handDoubleDown1Win },{ p.handDoubleDown1Loss },{ p.handDoubleDown1Push },{ p.handDoubleDown2Win },{ p.handDoubleDown2Loss },{p.handDoubleDown2Push },{ p.handSplitOffered },{ p.handSplitAccepted },{ p.handSplitDeclined },{ p.handSplitWin },{ p.handSplitLoss },{ p.handSplitPush },{ p.handSplit2Win },{ p.handSplit2Loss },{ p.handSplit2Push },{ p.handBust },{ p.handBust2 },{ dealer.handBust },{ p.handNaturalBJ },{ dealer.handNaturalBJ },{ p.handPushNaturalBJ },{ p.handInsuranceOffered },{ p.handInsuranceDeclined },{ p.handInsuranceBought },{ p.handInsuranceWin },{ p.handInsuranceLoss },{ p.handInsuranceDeclineLoss },{first2Total},{dealerFirstCard}");
			}
		}

		private void DisplayStats()
		{
			int horzPosition = 10;
			double delta = 0;
			Console.ForegroundColor = ConsoleColor.DarkBlue;
			Console.SetCursorPosition(10, 1);
			Console.WriteLine("Starting Bankroll:  {0,7:n0}   Bet Size  {1,5:n0}   Hands Played  {2,9:n0}", initialBankTotal, betAmount, runTimes);
			foreach (PlayerModel p in players)
			{
				Console.SetCursorPosition(horzPosition, 4);
				Console.WriteLine("    Total Number of Hands:  {0,7:n0}", p.handNumber);
				Console.SetCursorPosition(horzPosition, 5);
				Console.WriteLine("               Player Win:  {0,7:n0} {1,5:f1}% ", p.totalWin, (double)p.totalWin / (double)p.handNumber * 100);
				Console.SetCursorPosition(horzPosition, 6);
				Console.WriteLine("              Player Loss:  {0,7:n0} {1,5:f1}% ", p.totalLoss, (double)p.totalLoss / (double)p.handNumber * 100);
				Console.SetCursorPosition(horzPosition, 7);
				Console.WriteLine("             Pushed Hands:  {0,7:n0} {1,5:f1}% ", p.totalPush, (double)p.totalPush / (double)p.handNumber * 100);
				Console.SetCursorPosition(horzPosition, 9);
				Console.WriteLine("Player Natural Blackjacks:  {0,7:n0} {1,5:f1}% ", p.totalNaturalBJ, (double)p.totalNaturalBJ / (double)p.handNumber*100);
				Console.SetCursorPosition(horzPosition, 10);
				Console.WriteLine("Dealer Natural Blackjacks:  {0,7:n0} {1,5:f1}% ", p.totalDealerNaturalBJ, (double)p.totalDealerNaturalBJ / (double)p.handNumber * 100);
				Console.SetCursorPosition(horzPosition, 11);
				Console.WriteLine("  Push Natural Blackjacks:  {0,7:n0} {1,5:f1}% ", p.totalPushNaturalBJ, (double)p.totalPushNaturalBJ / (double)p.handNumber * 100);
				Console.SetCursorPosition(horzPosition, 13);
				delta = p.totalDoubleDownOffered / p.handNumber * 100;
				Console.WriteLine("      Offered Double Down:  {0,7:n0} {1,5:f1}% ", p.totalDoubleDownOffered, delta);
				Console.SetCursorPosition(horzPosition, 14);
				if (p.totalDoubleDownOffered != 0) { delta = p.totalDoubleDownAccepted / p.totalDoubleDownOffered * 100; } else { delta = 0; }
				Console.WriteLine("     Accepted Double Down:  {0,7:n0} {1,5:f1}% ", p.totalDoubleDownAccepted, delta);
				Console.SetCursorPosition(horzPosition, 15);
				if (p.totalDoubleDownOffered != 0) { delta = p.totalDoubleDownDeclined / p.totalDoubleDownOffered * 100; } else { delta = 0; }
				Console.WriteLine("     Declined Double Down:  {0,7:n0} {1,5:f1}% ", p.totalDoubleDownDeclined, delta);
				Console.SetCursorPosition(horzPosition, 16);
				if ((p.totalDoubleDown1Win + p.totalDoubleDown1Loss + p.totalDoubleDown1Push) != 0) { delta = p.totalDoubleDown1Win / (p.totalDoubleDown1Win + p.totalDoubleDown1Loss + p.totalDoubleDown1Push) * 100; } else { delta = 0; }
				Console.WriteLine("     Player 1 DD Hand Win:  {0,7:n0} {1,5:f1}% ", p.totalDoubleDown1Win, delta);
				Console.SetCursorPosition(horzPosition, 17);
				if ((p.totalDoubleDown1Win + p.totalDoubleDown1Loss + p.totalDoubleDown1Push) != 0) { delta = p.totalDoubleDown1Loss / (p.totalDoubleDown1Win + p.totalDoubleDown1Loss + p.totalDoubleDown1Push) * 100; } else { delta = 0; }
				Console.WriteLine("    Player 1 DD Hand Loss:  {0,7:n0} {1,5:f1}% ", p.totalDoubleDown1Loss, delta);
				Console.SetCursorPosition(horzPosition, 18);
				if ((p.totalDoubleDown1Win + p.totalDoubleDown1Loss + p.totalDoubleDown1Push) != 0) { delta = p.totalDoubleDown1Push / (p.totalDoubleDown1Win + p.totalDoubleDown1Loss + p.totalDoubleDown1Push) * 100; } else { delta = 0; }
				Console.WriteLine("    Player 1 DD Hand Push:  {0,7:n0} {1,5:f1}% ", p.totalDoubleDown1Push, delta);
				Console.SetCursorPosition(horzPosition, 19);
				if ((p.totalDoubleDown2Win + p.totalDoubleDown2Loss + p.totalDoubleDown2Push) != 0) { delta = p.totalDoubleDown2Win / (p.totalDoubleDown2Win + p.totalDoubleDown2Loss + p.totalDoubleDown2Push) * 100; } else { delta = 0; }
				Console.WriteLine("     Player 2 DD Hand Win:  {0,7:n0} {1,5:f1}% ", p.totalDoubleDown2Win, delta);
				Console.SetCursorPosition(horzPosition, 20);
				if ((p.totalDoubleDown2Win + p.totalDoubleDown2Loss + p.totalDoubleDown2Push) != 0) { delta = p.totalDoubleDown2Loss / (p.totalDoubleDown2Win + p.totalDoubleDown2Loss + p.totalDoubleDown2Push) * 100; } else { delta = 0; }
				Console.WriteLine("    Player 2 DD Hand Loss:  {0,7:n0} {1,5:f1}% ", p.totalDoubleDown2Loss, delta);
				Console.SetCursorPosition(horzPosition, 21);
				if ((p.totalDoubleDown2Win + p.totalDoubleDown2Loss + p.totalDoubleDown2Push) != 0) { delta = p.totalDoubleDown2Push / (p.totalDoubleDown2Win + p.totalDoubleDown2Loss + p.totalDoubleDown2Push) * 100; } else { delta = 0; }
				Console.WriteLine("    Player 2 DD Hand Push:  {0,7:n0} {1,5:f1}% ", p.totalDoubleDown2Push, delta);
				Console.SetCursorPosition(horzPosition, 23);
				delta = p.totalSplitOffered / p.handNumber * 100;
				Console.WriteLine("      Offered Split Cards:  {0,7:n0} {1,5:f1}% ", p.totalSplitOffered, delta);
				Console.SetCursorPosition(horzPosition, 24);
				if (p.totalSplitOffered != 0) { delta = p.totalSplitAccepted / p.totalSplitOffered * 100; } else { delta = 0; }
				Console.WriteLine("       Accept Split Cards:  {0,7:n0} {1,5:f1}% ", p.totalSplitAccepted, delta);
				Console.SetCursorPosition(horzPosition, 25);
				if (p.totalSplitOffered != 0) { delta = p.totalSplitDeclined / p.totalSplitOffered * 100; } else { delta = 0; }
				Console.WriteLine("     Declined Split Cards:  {0,7:n0} {1,5:f1}% ", p.totalSplitDeclined, delta);
				Console.SetCursorPosition(horzPosition, 26);
				if ((p.totalSplitWin + p.totalSplitLoss + p.totalSplitPush) != 0) { delta = p.totalSplitWin / (p.totalSplitWin + p.totalSplitLoss + p.totalSplitPush) * 100; } else { delta = 0; }
				Console.WriteLine("      Player 1st Hand Win:  {0,7:n0} {1,5:f1}% ", p.totalSplitWin, delta);
				Console.SetCursorPosition(horzPosition, 27);
				if ((p.totalSplitWin + p.totalSplitLoss + p.totalSplitPush) != 0) { delta = p.totalSplitLoss / (p.totalSplitWin + p.totalSplitLoss + p.totalSplitPush) * 100; } else { delta = 0; }
				Console.WriteLine("     Player 1st Hand Loss:  {0,7:n0} {1,5:f1}% ", p.totalSplitLoss, delta);
				Console.SetCursorPosition(horzPosition, 28);
				if ((p.totalSplitWin + p.totalSplitLoss + p.totalSplitPush) != 0) { delta = p.totalSplitPush / (p.totalSplitWin + p.totalSplitLoss + p.totalSplitPush) * 100; } else { delta = 0; }
				Console.WriteLine("     Player 1st Hand Push:  {0,7:n0} {1,5:f1}% ", p.totalSplitPush, delta);
				Console.SetCursorPosition(horzPosition, 29);
				if ((p.totalSplit2Win + p.totalSplit2Loss + p.totalSplit2Push) != 0) { delta = p.totalSplit2Win / (p.totalSplit2Win + p.totalSplit2Loss + p.totalSplit2Push) * 100; } else { delta = 0; }
				Console.WriteLine("      Player 2nd Hand Win:  {0,7:n0} {1,5:f1}% ", p.totalSplit2Win, delta);
				Console.SetCursorPosition(horzPosition, 30);
				if ((p.totalSplit2Win + p.totalSplit2Loss + p.totalSplit2Push) != 0) { delta = p.totalSplit2Loss / (p.totalSplit2Win + p.totalSplit2Loss + p.totalSplit2Push) * 100; } else { delta = 0; }
				Console.WriteLine("     Player 2nd Hand Loss:  {0,7:n0} {1,5:f1}% ", p.totalSplit2Loss, delta);
				Console.SetCursorPosition(horzPosition, 31);
				if ((p.totalSplit2Win + p.totalSplit2Loss + p.totalSplit2Push) != 0) { delta = p.totalSplit2Push / (p.totalSplit2Win + p.totalSplit2Loss + p.totalSplit2Push) * 100; } else { delta = 0; }
				Console.WriteLine("     Player 2nd Hand Push:  {0,7:n0} {1,5:f1}% ", p.totalSplit2Push, delta);
				Console.SetCursorPosition(horzPosition, 33);
				if (p.totalInsuranceOffered != 0) { delta = p.totalInsuranceOffered / p.handNumber * 100; } else { delta = 0; }
				Console.WriteLine("        Offered Insurance:  {0,7:n0} {1,5:f1}% ", p.totalInsuranceOffered, delta);
				Console.SetCursorPosition(horzPosition, 34);
				if (p.totalInsuranceOffered != 0) { delta = p.totalInsuranceDeclined / p.totalInsuranceOffered * 100; } else { delta = 0; }
				Console.WriteLine("       Declined Insurance:  {0,7:n0} {1,5:f1}% ", p.totalInsuranceDeclined, delta);
				Console.SetCursorPosition(horzPosition, 35);
				Console.WriteLine("Declined Insurance Losses:  {0,7:n0} {1,5:f1}% ", p.totalInsuranceDeclineLoss, (double)p.totalInsuranceDeclineLoss / (double)p.totalDealerNaturalBJ*100);
				Console.SetCursorPosition(horzPosition, 36);
				if (p.totalInsuranceOffered != 0) { delta = p.totalInsuranceBought / p.totalInsuranceOffered * 100; } else { delta = 0; }
				Console.WriteLine("         Bought Insurance:  {0,7:n0} {1,5:f1}% ", p.totalInsuranceBought, delta);
				Console.SetCursorPosition(horzPosition, 37);
				if (p.totalInsuranceBought != 0) { delta = p.totalInsuranceWin / p.totalInsuranceBought * 100; } else { delta = 0; }
				Console.WriteLine("        Won Insurance Bet:  {0,7:n0} {1,5:f1}% ", p.totalInsuranceWin, delta);
				Console.SetCursorPosition(horzPosition, 38);
				if (p.totalInsuranceBought != 0) { delta = p.totalInsuranceLoss / p.totalInsuranceBought * 100; } else { delta = 0; }
				Console.WriteLine("       Lost Insurance Bet:  {0,7:n0} {1,5:f1}% ", p.totalInsuranceLoss, delta);
				horzPosition += 50;
			}
		}

		private void DisplayBank()
		{
			Console.SetCursorPosition(2, 3);
			Console.ForegroundColor = ConsoleColor.Black;
			Console.Write(player1.handNumber + 1);
			int headerBankPosition = 10;
			double delta = 0;
			foreach (PlayerModel p in players)
			{
				if (p.bankroll > initialBankTotal)
				{
					Console.ForegroundColor = ConsoleColor.DarkGreen;
				}
				else if (p.bankroll < initialBankTotal)
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.DarkMagenta;
				}
				Console.SetCursorPosition(headerBankPosition, 3);
				delta = (p.bankroll - initialBankTotal) / initialBankTotal * 100;
				Console.Write("                 Bankroll: {0,7:c0} {1,5:f1}%   ", p.bankroll, delta);
				headerBankPosition += 50;
			}
		}

		private void DealerHitMe()
		{
			dealer.cardNumber += 1;
			dealer.hand[dealer.cardNumber] = GetDeck[nextCardIndex];
			nextCardIndex++;
			DealerHandTotal();
		}

		private void DealerBusted(PlayerModel p)
		{
			dealer.totalBust++;
			dealer.handBust = 1;
			if (p.total <= 21)
			{
				p.betResult = p.bet * 2;
				p.totalWin++;
				p.handWin = 1;
				//if (p.isSplit)
				//{
				//	p.handSplitWin = 1;
				//}
			}
			if (p.isSplit && p.total2 <= 21)
			{
				p.bet2Result = p.bet2 * 2;
				p.totalWin2++;
				p.handWin2 = 1;
				//p.handSplit2Win = 1;
			}
			p.gameOver = true;
			BankUpdate(p);
		}

		private void DealerTurn()
		{
			if (hitOnSoft17 && IfSoftAce(dealer.hand))
			{
				while (dealer.total < 18 && dealer.total <= 21)
				{
					DealerHitMe();
				}
			}
			else
			{
				while (dealer.total < 17 && dealer.total <= 21)
				{
					DealerHitMe();
				}
			}
			foreach (PlayerModel p in players)
			{
				if (!p.isSplit && !p.gameOver)
				{
					if (dealer.total > 21)
					{
						DealerBusted(p);
					}
					else if (p.total > dealer.total)
					{
						p.betResult = p.bet * 2;
						p.handWin = 1;
						p.totalWin++;
						p.gameOver = true;
						BankUpdate(p);
					}
					else if (dealer.total > p.total)
					{
						p.betResult = 0;
						p.handLoss = 1;
						p.totalLoss++;
						p.gameOver = true;
						BankUpdate(p);
					}
					else if (p.total == dealer.total)
					{
						p.betResult = p.bet + 0;
						p.handPush = 1;
						p.totalPush++;
						p.gameOver = true;
						BankUpdate(p);
					}
				}
				else if (!p.gameOver)
				{
					if (dealer.total > 21)
					{
						DealerBusted(p);
					}
					else if (p.total > dealer.total && p.total < 22)
					{
						p.betResult = p.bet * 2;
						p.handWin = 1;
						//p.handSplitWin = 1;
						p.totalWin++;
					}
					else if (dealer.total > p.total)
					{
						p.betResult = 0;
						p.handLoss = 1;
						//p.handSplitLoss = 1;
						p.totalLoss++;
					}
					else if (dealer.total == p.total)
					{
						p.betResult = p.bet + 0;
						p.handPush = 1;
						//p.handSplitPush = 1;
						p.totalPush++;
					}
					if (p.total2 > dealer.total && p.total2 < 22)
					{
						p.bet2Result = p.bet2 * 2;
						p.handWin2 = 1;
						//p.handSplit2Win = 1;
						p.totalWin2++;
					}
					else if (dealer.total > p.total2 && dealer.total < 22)
					{
						p.bet2Result = 0;
						p.handLoss2 = 1;
						//p.handSplit2Loss = 1;
						p.totalLoss2++;
					}
					else if (dealer.total == p.total2 && dealer.total < 22)
					{
						p.bet2Result = p.bet2 + 0;
						p.handPush2 = 1;
						//p.handSplit2Push = 1;
					}
					BankUpdate(p);
				}
			}
		}

		private bool IfSoftAce(Cards[] hnd)
		{
			int subTotal = 0;
			int aces = 0;

			foreach (Cards total in hnd)
			{
				if (total != null)
				{
					subTotal += (int)total.MyValue;
					if ((int)total.MyValue == 11)
					{
						aces++;
					}
				}
			}
			if ((aces == 1 && subTotal < 22) || (aces == 2 && subTotal < 32) || (aces == 3 && subTotal < 42) || (aces == 4 && subTotal < 52))
			{
				return true;
			}
			return false;
		}

		private void InsuranceOption(PlayerModel p)
		{
			p.insurance = true;
			p.totalInsuranceBought++;
			p.handInsuranceBought = 1;
			p.insuranceBet = p.bet * .5;
			p.betTotal += p.insuranceBet;
			if (dealer.total == 21)
			{
				p.totalInsuranceWin++;
				p.handInsuranceWin = 1;
				dealer.totalNaturalBJ++;
				dealer.handNaturalBJ = 1;
				p.insuranceBetResult = p.insuranceBet + p.insuranceBet * 2;
				if (p.total == 21)
				{
					p.betResult = p.bet + 0;
					p.totalPush++;
					p.handPush = 1;
				}
				else
				{
					p.betResult = -p.bet;
					p.betResult = 0;
					p.totalLoss++;
					p.handLoss = 1;
				}
				p.turnOver = true;
				p.gameOver = true;
				BankUpdate(p);
			}
			else
			{
				p.totalInsuranceLoss++;
				p.handInsuranceLoss = 1;
				p.insuranceBetResult = 0;
			}
		}

		private void HitMe(PlayerModel p)
		{
			if (!p.secondHand)
			{
				p.firstOption = false;
				p.cardNumber += 1;
				p.hand[p.cardNumber] = GetDeck[nextCardIndex];
				nextCardIndex++;
				if (playGame)
				{
					y = playerCardPosition;
					x = p.cardNumber;
					DrawCards.DrawCardOutline(x, y);
					DrawCards.DrawCardSuitValue(p.hand[p.cardNumber], x, y);
					y = playerMessagePosition;
				}
			}
			else
			{
				p.first2Option = false;
				p.card2Number += 1;
				p.hand2[p.card2Number] = GetDeck[nextCardIndex];
				nextCardIndex++;
				if (playGame)
				{
					y = player2CardPosition;
					x = p.card2Number;
					DrawCards.DrawCardOutline(x, y);
					DrawCards.DrawCardSuitValue(p.hand2[p.card2Number], x, y);
					y = player2MessagePosition;
				}
			}
			HandTotal(p);
		}

		private void PlayerBusted(PlayerModel p)
		{
			if (!p.isSplit || !p.secondHand)
			{
				p.totalBust++;
				p.handBust = 1;
				p.totalLoss++;
				p.handLoss = 1;
				p.betResult = 0;
				if (!p.isSplit)
				{
					p.turnOver = true;
					p.gameOver = true;
					BankUpdate(p);
				}
				else
				{
					//p.totalSplitLoss++;
					//p.handSplitLoss = 1;
					p.secondHand = true;
				}
				HandTotal(p);
			}
			else
			{
				p.bet2Result = 0;
				HandTotal(p);
				p.totalBust++;
				p.totalLoss2++;
				p.handBust2 = 1;
				p.handLoss2 = 1;
				//p.handSplit2Loss = 1;
				if (p.total > 21 && p.total2 > 21)
				{
					p.turnOver = true;
					p.gameOver = true;
					BankUpdate(p);
				}
				else
				{
					p.turnOver = true;
				}
			}
		}

		private void DoubleDown(PlayerModel p)
		{
			if (!p.isSplit || !p.secondHand)
			{
				p.betTotal += p.bet;
				p.bet = p.bet * 2;
			}
			else
			{
				p.betTotal += p.bet2;
				p.bet2 = p.bet2 * 2;
			}
			HitMe(p);
			if (!p.isSplit || !p.secondHand)
			{
				if (p.total > 21)
				{
					PlayerBusted(p);
				}
				else
				{
					if (!p.isSplit)
					{
						p.turnOver = true;
					}
				}
			}
			else
			{
				if (p.total2 > 21)
				{
					PlayerBusted(p);
				}
				else
				{
					p.turnOver = true;
				}
			}
		}

		private void InitializeSplitHands(PlayerModel p)
		{
			p.allowSplit = false;
			//p.firstOption = false;
			p.hand2[0] = p.hand[1];
			p.hand[1] = GetDeck[nextCardIndex];
			nextCardIndex++;
			p.hand2[1] = GetDeck[nextCardIndex];
			nextCardIndex++;
			p.bet2 = betAmount;
			p.betTotal += p.bet2;
			p.card2Number = 1;
			HandTotal(p);
			if ((p.total == 9 || p.total == 10 || p.total == 11) && allowDoubleDown)
			{
				if ((int)p.hand[0].MyValue != 5)
				{
					p.totalDoubleDownOffered++;
					p.handDoubleDownOffered = 1; 
				}
				AllowDD(p, p.total); 
			}
			if ((int)p.hand[0].MyValue == 11)
			{
				p.secondHand = true;
				p.turnOver = true;
			}
		}

		private void InitialDeal()
		{
			foreach (PlayerModel deal in players)
			{
				deal.hand[0] = GetDeck[nextCardIndex];
				nextCardIndex++;
			}
			dealer.hand[0] = GetDeck[nextCardIndex];
			nextCardIndex++;
			foreach (PlayerModel deal in players)
			{
				deal.hand[1] = GetDeck[nextCardIndex];
				nextCardIndex++;
				deal.cardNumber = 1;
				HandTotal(deal);
			}
			dealer.hand[1] = GetDeck[nextCardIndex];
			nextCardIndex++;
			dealer.cardNumber = 1;

			if ((int)dealer.hand[0].MyValue == 11 && offerInsurance)
			{
				foreach (PlayerModel deal in players)
				{
					if (deal.total != 21)
					{
						deal.handInsuranceOffered = 1;
						deal.totalInsuranceOffered++; 
					}
				}
			}
			DealerHandTotal();
			foreach (PlayerModel p in players)
			{
				if (p.total == 21 && dealer.total != 21)
				{
					p.betResult = p.bet + p.bet * 1.5;
					p.totalNaturalBJ++;
					p.handNaturalBJ = 1;
					p.totalWin++;
					p.handWin = 1;
					p.turnOver = true;
					p.gameOver = true;
					if (playGame == true)
					{
						GameInitialDeal(p);
					}
					else
					{
						BankUpdate(p);
					}
				}
				else if (p.total == 21 && dealer.total == 21)
				{
					p.betResult = p.bet;
					p.totalNaturalBJ++;
					p.handNaturalBJ = 1;
					p.totalDealerNaturalBJ++;
					p.handDealerNaturalBJ = 1;
					p.totalPushNaturalBJ++;
					p.handPushNaturalBJ = 1;
					p.totalPush++;
					p.handPush = 1;
					p.turnOver = true;
					p.gameOver = true;
					if (playGame == true)
					{
						GameInitialDeal(p);
					}
					else
					{
						BankUpdate(p);
					}
				}
			}
		}

		public void InitializeConsoleDisplay()
		{

			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.Clear();
		}

		private void SetUpBankroll(PlayerModel p)
		{
			p.initialBankroll = p.bankroll;
			p.betTotal += p.bet;
			p.bankroll -= p.betTotal;
		}

		private void BankUpdate(PlayerModel player)
		{
			player.betWon = player.betResult + player.bet2Result + player.insuranceBetResult;
			player.bankroll = player.initialBankroll - player.betTotal + player.betWon;
			player.betResult = -player.betTotal + player.betWon;
		}

		private void ResetGame()
		{
			nextMove = "";
			dealer.hand = new Cards[10];
			dealer.cardNumber = 0;
			dealer.total = 0;
			dealer.totalNaturalBJ = 0;
			dealer.handBust = 0;
			dealer.handNaturalBJ = 0;


			foreach (PlayerModel p in players)
			{
				p.getInsurance = false;
				p.allowSplit = false;
				p.allowDD = false;
				p.hand = new Cards[10];
				p.hand2 = new Cards[10];
				p.cardNumber = 0;
				p.card2Number = 0;
				p.bet = betAmount;
				p.bet2 = 0;
				p.betResult = 0;
				p.bet2Result = 0;
				p.betTotal = 0;
				p.betWon = 0;
				p.insuranceBet = 0;
				p.initialBankroll = 0;
				p.insuranceBetResult = 0;
				p.gameOver = false;
				p.turnOver = false;
				p.firstOption = true;
				p.first2Option = true;
				p.secondHand = false;
				p.isSplit = false;
				p.insurance = false;
				p.doubleDownFlag = 0;
				p.doubleDown2Flag = 0;
				p.insuranceFlag = 0;

				p.total = 0;
				p.total2 = 0;
				p.handWin = 0;
				p.handLoss = 0;
				p.handPush = 0;
				p.handWin2 = 0;
				p.handLoss2 = 0;
				p.handPush2 = 0;
				p.handSplitWin = 0;
				p.handSplitLoss = 0;
				p.handSplitPush = 0;
				p.handSplit2Win = 0;
				p.handSplit2Loss = 0;
				p.handSplit2Push = 0;
				p.handDoubleDown1Win = 0;
				p.handDoubleDown1Loss = 0;
				p.handDoubleDown1Push = 0;
				p.handDoubleDown2Win = 0;
				p.handDoubleDown2Loss = 0;
				p.handDoubleDown2Push = 0;
				p.handInsuranceWin = 0;
				p.handInsuranceLoss = 0;
				p.handInsuranceDeclineLoss = 0;
				p.handDoubleDownOffered = 0;
				p.handDoubleDownAccepted = 0;
				p.handDoubleDownDeclined = 0;
				p.handSplitOffered = 0;
				p.handSplitAccepted = 0;
				p.handSplitDeclined = 0;
				p.handBust = 0;
				p.handBust2 = 0;
				p.handNaturalBJ = 0;
				p.handPushNaturalBJ = 0;
				p.handInsuranceOffered = 0;
				p.handInsuranceBought = 0;
				p.handInsuranceDeclined = 0;
			}
		}

		private void DealerHandTotal()
		{
			dealer.total = 0;
			int subTotal = 0;
			int aces = 0;
			bool acesFlag = false;

			foreach (Cards total in dealer.hand)
			{
				if (total != null)
				{
					subTotal += (int)total.MyValue;
					if ((int)total.MyValue == 11)
					{
						aces++;
					}
				}
			}
			foreach (Cards tot in dealer.hand)
			{
				if (tot != null)
				{
					if ((int)tot.MyValue == 11)
					{
						if (subTotal > 21)
						{
							if ((aces == 2 && subTotal < 32 || aces == 3 && subTotal < 42 || aces == 4 && subTotal < 52) && !acesFlag)
							{
								dealer.total += (int)tot.MyValue;
								acesFlag = true;
							}
							else
							{
								dealer.total += 1;
							}
						}
						else
						{
							dealer.total += (int)tot.MyValue;
						}
					}
					else
					{
						dealer.total += (int)tot.MyValue;
					}
				}
				if (playGame)
				{
					Console.SetCursorPosition(20, dealerCardPosition - 1);
					if (!showDealCard && !gamePlayer1.gameOver)
					{
						Console.Write("Unknown");
					}
					else
					{
						Console.Write(dealer.total + "       ");
					}
					Console.WriteLine();
					Console.SetCursorPosition(0, playerMessagePosition);
				}
			}
		}

		private void HandTotal(PlayerModel player)
		{
			player.total = 0;
			int subTotal = 0;
			int aces = 0;
			bool acesFlag = false;
			if (player.isSplit)
			{
				player.total2 = 0;
				foreach (Cards total in player.hand2)
				{
					if (total != null)
					{
						subTotal += (int)total.MyValue;
						if ((int)total.MyValue == 11)
						{
							aces++;
						}
					}
				}
				foreach (Cards tot in player.hand2)
				{
					if (tot != null)
					{
						if ((int)tot.MyValue == 11)
						{
							if (subTotal > 21)
							{
								if ((aces == 2 && subTotal < 32 || aces == 3 && subTotal < 42 || aces == 4 && subTotal < 52) && !acesFlag)
								{
									player.total2 += (int)tot.MyValue;
									acesFlag = true;
								}
								else
								{
									player.total2 += 1;
								}
							}
							else
							{
								player.total2 += (int)tot.MyValue;
							}
						}
						else
						{
							player.total2 += (int)tot.MyValue;
						}
					}
				}
			}
			subTotal = 0;
			aces = 0;
			acesFlag = false;
			foreach (Cards total in player.hand)
			{
				if (total != null)
				{
					subTotal += (int)total.MyValue;
					if ((int)total.MyValue == 11)
					{
						aces++;
					}
				}
			}
			foreach (Cards tot in player.hand)
			{
				if (tot != null)
				{
					if ((int)tot.MyValue == 11)
					{
						if (subTotal > 21)
						{
							if ((aces == 2 && subTotal < 32 || aces == 3 && subTotal < 42 || aces == 4 && subTotal < 52) && !acesFlag)
							{
								player.total += (int)tot.MyValue;
								acesFlag = true;
							}
							else
							{
								player.total += 1;
							}
						}
						else
						{
							player.total += (int)tot.MyValue;
						}
					}
					else
					{
						player.total += (int)tot.MyValue;
					}
				}
			}
			if (playGame)
			{
				DisplayHandTotal(player);
			}
		}

		private void DisplayHandTotal(PlayerModel p)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;

			Console.SetCursorPosition(20, playerCardPosition - 1);
			Console.Write(p.total);

			if (p.isSplit)
			{
				Console.SetCursorPosition(20, player2CardPosition - 1);
				Console.Write(p.total2);
				Console.SetCursorPosition(0, player2MessagePosition);
			}
			else
			{
				Console.WriteLine();
				Console.SetCursorPosition(0, playerMessagePosition);
			}
		}
	}
}
