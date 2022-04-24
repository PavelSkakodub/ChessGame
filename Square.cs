
using System.Collections.Generic;

namespace ChessRules
{
    struct Square //структура клетки
    {
        public static Square none = new Square(-1, -1); //знач-е клетки за пределами доски

        public int x { get; private set; }
        public int y { get; private set; }
        public Square(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        public Square(string name) //e2,h8.g5 или др команда
        {
            if (name.Length == 2 && name[0] >= 'a' && name[0] <= 'h' && name[1] >= '1' && name[1] <= '8') //проверка на корректность команды
            {
                x = name[0] - 'a'; //'a'-'a' =0, 'b'-'a'=1 и т.д
                y = name[1] - '1'; //'1' -'1' =0, '8' -'1'=7
            }
            else this = none; //иначе клетка не сущ-ет на доске
        }

        public string Name //получает имя клетки ввиде команды (a2,g5,h7 и т.п)
        {
            get
            {
                if (OnBoard()) //если есть на доске
                {
                    //если x=0 будет а, если x=1 будет b и т.д
                    return ((char)('a' + x)).ToString() + (y + 1).ToString();
                }
                else 
                    return "-"; //иначе нет клетки
            }
        }

        public bool OnBoard() //проверка, находится ли наша фигура на доске
        {
            return (x >= 0 && x < 8) && (y >= 0 && y < 8);
        }

        public static IEnumerable<Square> YieldBoardSquares() //возвращает строки, содержащие какой ход можно сделатье
        {
            //собираем все правильные ходы через проверку каждой клетки
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    yield return new Square(x,y); //возврат каждого элемента(клетки) по одному
                }
            }
        }

        public static bool operator == (Square a,Square b) //оператор сравнения 2 клеток
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Square a, Square b) //оператор сравнения 2 клеток
        {
            return !(a == b); //отрицание от равенства клеток
        }
    }
}
