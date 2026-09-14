using System;
using System.Collections.Generic;
using System.Threading;
class Program
{
	static string signs = " XO";
	static int on, delay = 700, end, row, col;
	static bool player, bot = true, turns = true, midDo, corDo, sideDo;
	static int[,] places;  //{ false(kezdetben)/true(ha foglalt) }
	static int[,] points;   //{ 1.sor, 2.sor, 3.sor, 1.oszlop, 2.oszlop, 3.oszlop, fentről-le, lentről-fel }
	static readonly string[] set = { "Start", " Bot ", "Delay", "Turns" };
	static readonly string[] res = { "The game ended in a draw.", "The player with the crosses won the game.", "The player with the noughts won the game." };
	static List<(int, int)> corner;
	static List<(int, int)> side;
	static void Main()//A játék indítása és beállítások
	{
		//Indítás:
		Console.WindowWidth = 64;
		Console.WindowHeight = 22;
		Console.Title = "Console Tic-Tac-Toe";
		Console.CursorVisible = false;
		cw("Console Tic-Tac-Toe\nby Trefi", 200);
		Thread.Sleep(400);
		cw(" :)", 300);
		Thread.Sleep(1500);
		Console.SetCursorPosition(0, 0);
		//Maga a játék:
		string choise = "s";
		do
		{
			//Főmenü:
			if (choise == "s")
				Settings();
			//Reset:
			on = 0; end = 0; row = 0; col = 0;
			player = false; midDo = true; corDo = true; sideDo = true;
			points = new int[2, 8];
			places = new int[3, 3];
			corner = new List<(int, int)> { (0, 0), (0, 2), (2, 0), (2, 2) };
			side = new List<(int, int)> { (0, 1), (1, 0), (1, 2), (2, 1) };
			//Egy meccs:
			Game();
			//Eredmények:
			cw($"\n{res[end - 9]}\nDo you want to play again?\ny - Yes\nn - No\ns - Settings\nYour answer: ", 35);
			choise = Console.ReadLine();
			while (choise != "y" && choise != "n" && choise != "s")
			{
				Erase(19, 13, choise.Length);
				choise = Console.ReadLine();
			}
		} while (choise != "n");
	}
	static void cw(string text, int delay)//Felirat
	{
		for (int i = 0; i < text.Length; ++i)
		{
			Console.Write(text[i]);
			Thread.Sleep(delay);
		}
	}
	static void Erase(int row, int col, int len)
	{
		Console.SetCursorPosition(col, row);
		for (int i = 0; i < len; ++i)
			Console.Write(" ");
		Console.SetCursorPosition(col, row);
	}
	static void Show(int on)//Menüpontok kijelzése
	{
		Console.SetCursorPosition(0, 2);
		for (int i = 0; i < set.Length; ++i)
		{
			Console.Write("   ");
			if (on == i)
			{
				Console.BackgroundColor = ConsoleColor.Gray;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.Write(set[i]);
				Console.ResetColor();
			}
			else
				Console.Write(set[i]);
		}
	}
	static void Switch(ref bool b, string title, string t, string f)//Kiírás
	{
		Console.Write(title);
		bool choise;
		do
		{
			if (b)
				Console.Write(t);
			else
				Console.Write(f);
			if (choise = Console.ReadKey().Key != ConsoleKey.Enter)
			{
				Console.Write("\b \b");
				b = !b;
				Console.CursorLeft = title.Length;
			}
		} while (choise);
		Erase(3, 0, title.Length + t.Length);
	}
	static void Settings()//Beállítások
	{
		Console.Clear();
		Console.WriteLine("You can choose a cell by using the ARROWS\nand select it with the ENTER key.");
		ConsoleKeyInfo _Key;
		do
		{
			Show(on);
			do
			{
				bool yes = true;
				_Key = Console.ReadKey();
				Console.Write("\b \b");
				if (_Key.Key == ConsoleKey.LeftArrow && on > 0)
					--on;
				else if (_Key.Key == ConsoleKey.RightArrow && on < 3)
					++on;
				else if (_Key.Key != ConsoleKey.Enter)
					yes = false;
				if (yes)
					Show(on);
			} while (_Key.Key != ConsoleKey.Enter);
			Console.WriteLine();
			if (on == 1)
				Switch(ref bot, "Do you wish to play against a bot? (default: Yes) - ", "Yes", "No ");
			else if (on == 2)
			{
				bool error;
				string title = "Set the delay of the bots move! (default: 700ms) - ", num;
				Console.Write(title);
				do
				{
					if (error = !int.TryParse(num = Console.ReadLine(), out delay) || delay < 1 || delay > 2000)
						Erase(3, title.Length, num.Length);
				} while (error);
				Erase(3, 0, title.Length + num.Length);
			}
			else if (on == 3)
				Switch(ref turns, "Which player do you want to be? (default: Crosses) - ", "Crosses", "Noughts");
		} while (on != 0);
	}
	static void Game()//A játék menete
	{
		Console.Clear();
		Console.WriteLine("┌───────┬───────┬───────┐");
		for (int i = 1; i < 12; ++i)
		{
			if (i % 4 == 0)
				Console.WriteLine("├───────┼───────┼───────┤");
			else
				Console.WriteLine("│       │       │       │");
		}
		Console.WriteLine("└───────┴───────┴───────┘");
		if (bot && !turns)
			Add(1, 1);
		Console.BackgroundColor = ConsoleColor.Gray;
		Console.ForegroundColor = ConsoleColor.Black;
		Rewrite();
		while (end < 9)
		{
			ConsoleKeyInfo _Key;
			do
			{
				//Bekért billentyűparancs éretelmezése:
				Console.ResetColor();
				_Key = Console.ReadKey();
				Console.Write("\b \b");
				Rewrite();
				if (_Key.Key == ConsoleKey.UpArrow && row > 0)
					--row;
				else if (_Key.Key == ConsoleKey.DownArrow && row < 2)
					++row;
				else if (_Key.Key == ConsoleKey.LeftArrow && col > 0)
					--col;
				else if (_Key.Key == ConsoleKey.RightArrow && col < 2)
					++col;
				Console.BackgroundColor = ConsoleColor.Gray;
				Console.ForegroundColor = ConsoleColor.Black;
				Rewrite();
			} while (_Key.Key != ConsoleKey.Enter || places[row, col] != 0);
			//Az eredmény kijelzése:
			Add(row, col);
			//Az esetleges bot játékának kijelzése:
			if (bot && !Init(0, 3) && end < 9)
				Bot(Convert.ToInt32(player), Convert.ToInt32(!player));
		}
	}
	static void Rewrite()//Játékállás megjelenítése
	{
		for (int i = 1; i < 4; ++i)
		{
			Console.SetCursorPosition(col * 8 + 1, row * 4 + i);
			for (int j = 0; j < 7; ++j)
			{
				if (i == 2 && j == 3)
					Console.Write(signs[places[row, col]]);
				else
					Console.Write(" ");
			}
		}
		Console.SetCursorPosition(0, 13);
	}
	static void Add(int addRow, int addCol)//Lépés végrehajtása (sor, oszlop)
	{
		int p = Convert.ToInt32(player);
		//Lépés beírása:
		if (bot && player == turns)
			Thread.Sleep(delay);
		Console.SetCursorPosition(addCol * 8 + 4, addRow * 4 + 2);
		Console.Write(signs[p + 1]);
		Console.SetCursorPosition(0, 13);
		Console.ResetColor();
		//Lépés feljegyzése
		places[addRow, addCol] = p + 1;
		//Player állása és kiértékelés:
		++points[p, addRow];
		++points[p, addCol + 3];
		if (addRow == addCol)
			++points[p, 6];
		if (addRow == 2 - addCol || addRow == 1 && addCol == 1 || 2 - addRow == addCol)
			++points[p, 7];
		if (Init(p, 3))
			end = 9 + p;
		player = !player;
		++end;
	}
	static bool Init(int playerNum, int num)//Contains, a points mátrixra specializálva (játékos száma, keresett szám): igaz/hamis
	{
		for (int i = 0; i < 8; ++i)
		{
			if (points[playerNum, i] == num)
				return true;
		}
		return false;
	}
	static bool Do(int fst, int snd)//Lépje meg, ha szabályos (sor, oszlop): tudott lépni/nem tudott lépni
	{
		if (places[fst, snd] == 0)
		{
			Add(fst, snd);
			return false;
		}
		return true;
	}
	static void Gottem(int num)//Ha kijönne 3 (points egy sorának eleme)
	{
		if (num < 3)        //Sorban jön ki kettő
			for (int i = 0; i < 3 && Do(num, i); ++i) ;
		else if (num < 6)   //Oszlopban jön ki kettő
			for (int i = 0; i < 3 && Do(i, num - 3); ++i) ;
		else if (num == 6)  //Felső átlón jön ki kettő
			for (int i = 0; i < 3 && Do(i, i); ++i) ;
		else                //Alsó átlón jön ki kettő
			for (int i = 0; i < 3 && Do(2 - i, i); ++i) ;
	}
	static bool Fork(int p, int o)//Fork esetén (játékos, ellenfél): volt/nem volt
	{
		for (int i = 0; i < 3; ++i)
		{
			for (int j = 3; j < 6; ++j)
			{
				if (places[i, j - 3] == 0 && points[p, i] == 1 && points[p, j] == 1 && points[o, i] == 0 && points[o, j] == 0)
				{
					Add(i, j - 3);
					return true;
				}
			}
		}
		return false;
	}
	static bool Move(List<(int, int)> order)//Véletlenszerű lépés a lista elemeiből (halmaz): tudott lépni/nem tudott lépni
	{
		Random r = new Random();
		int rand, one, two;
		do
		{
			rand = r.Next(0, order.Count);
			one = order[rand].Item1;
			two = order[rand].Item2;
			if (places[one, two] == 0)
			{
				Add(one, two);
				return true;
			}
			else
				order.Remove((one, two));
		} while (order.Count > 0);
		return false;
	}
	static void Bot(int p, int o)//Bot játékos lépése
	{
		//Ha a bot nyerhetne egy lépésből:
		for (int i = 0; i < 8; ++i)
		{
			if (points[p, i] == 2 && points[o, i] == 0)
			{
				Gottem(i);
				end = 10;
				return;
			}
		}
		//Ha a játékos nyerhetne egy lépésből:
		for (int i = 0; i < 8; ++i)
		{
			if (points[o, i] == 2 && points[p, i] == 0)
			{
				Gottem(i);
				return;
			}
		}
		//Ha tudna fork-ot adni:
		if (Fork(p, o))
			return;
		//Ha az ellenfél tudna speciális fork-ot adni:
		if (end == 3)
		{
			if (points[o, 6] == 2 && points[p, 6] == 1 || points[o, 7] == 2 && points[p, 7] == 1)
			{
				if (points[1, 1] == 1)
					Move(side);
				else
					Move(corner);
				return;
			}
		}
		//Ha az ellenfél tudna más fork-ot adni:
		if (Fork(o, p))
			return;
		//Amúgy a megadott sorrendben rakjon, ha tud:
		if (midDo)  //Középre
		{
			if (!Do(1, 1))
				return;
			midDo = false;
		}
		if (corDo)  //Sarokra
		{
			if (corDo = Move(corner))
				return;
		}
		if (sideDo) //Oldalközépre
			Move(side);
		return;
	}
}