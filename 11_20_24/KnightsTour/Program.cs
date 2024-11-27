using System.Text.RegularExpressions;

namespace KnightsTour {
  internal class Program {
    static int[,] board;
    static (int dx, int dy)[] moves = { (2, 1), (2, -1), (1, 2), (1, -2), (-1, 2), (-1, -2), (-2, 1), (-2, -1) };
    static int nc, nr;
    static int board_count { get { return nc * nr; } }

    static void Main(string[] args) {
      set_board_size();

      string input;
      string[] start;
      int startX, startY;
      Regex r = new Regex(@"\s*\d+\,\s*\d+");

      do {
        Console.Write("Enter starting location \"x,y\" to find tour\nEnter \"board\" to change the size of the board\nEnter \"X\" to close\nInput: ");
        input = Console.ReadLine() ?? "";

        if(string.IsNullOrEmpty(input)) {
          Console.WriteLine("Invalid input. Try Again.");
        } 
        else if(string.Equals(input, "board")) {
          set_board_size();
        }
        else if(string.Equals(input, "X")) {
          break;
        } 
        else if(r.IsMatch(input)) {
          start = input.Split(',', StringSplitOptions.TrimEntries);
          if(!(int.TryParse(start[0], out startX) && int.TryParse(start[1], out startY))) {
            Console.WriteLine("Invalid input. Try Again.");
            continue;
          }
          Array.Clear(board);
          make_move(startX, startY);
          print_board();  
        }
        else {
          Console.WriteLine("Invalid input. Try Again.");
        }
      } while(true);

    }

    static void set_board_size() {
      string input;
      do {
        Console.Write("Set Board Columns: ");
        input = Console.ReadLine() ?? "";
      } while(!int.TryParse(input, out nc));

      do {
        Console.Write("Set Board Rows: ");
        input = Console.ReadLine() ?? "";
      } while(!int.TryParse(input, out nr));

      board = new int[nc, nr];
    }

    static void print_board() {
      int colSize = board_count.ToString().Length + 1;
      for(int i = 0; i < nc; i++) {
        for(int j = 0; j < nr; j++) {
          Console.Write(String.Format("{0," + colSize + "}", board[i, j]));
        }
        Console.WriteLine("\n");
      }
    }

    static bool check_valid_move(int x, int y, (int dx, int dy) move) {
      return x + move.dx >= 0
          && x + move.dx < nc
          && y + move.dy >= 0
          && y + move.dy < nr
          && board[x + move.dx, y + move.dy] == 0;
    }
    static bool make_move(int x, int y, int count = 0) {
      board[x, y] = ++count;

      if(count == board_count)
        return true;

      foreach((int dx, int dy) move in moves.Where(m=>check_valid_move(x,y,m)).OrderBy(m=>get_move_count(x+m.dx,y+m.dy)).ToList()) {
        if(make_move(x + move.dx, y + move.dy, count))
          return true;
      }
      board[x, y] = 0;
      return false;
    }

    static int get_move_count(int x, int y) {
      return moves.Where(m => check_valid_move(x, y, m)).Count();
    }

  }
}
