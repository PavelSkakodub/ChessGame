
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessRules
{
    class Board //класс доски с размещением фигур и перемещением
    {
        public string fen { get; protected set; }
        public Color moveColor { get; protected set; } //цвет текущего хода
        
        //возможности ракировки
        public bool canCastleA1 { get; protected set; } //Q
        public bool canCastleH1 { get; protected set; } //K
        public bool canCastleA8 { get; protected set; } //q
        public bool canCastleH8 { get; protected set; } //k

        public Square enpassant { get; protected set; } //наличие битова поля (2 хода у пешки вначале)
        public int drawNumber { get; protected set; } //номер хода для проверки правила 50 ходов
        public int moveNumber { get; protected set; } //кол-во сделанных ходов
        protected Figure[,] figures; //матрица фигур (protected для доступа к порождённому классу NextBoard)

        public Board(string fen)
        {
            this.fen = fen;
            figures = new Figure[8, 8]; //размер поля 8*8
            Init(); //задаём нач.условия
        }

        public Board Move(FigureMoving fm) //перемещение (ход) фигуры с возвратом новой доски
        {
            //возвращаем новую доску с сделанным ходом (от расширяющего класса NextBoard)
            return new NextBoard(fen,fm); 
        }

        public IEnumerable<FigureOnSquare> YieldMyFigureOnSquares() //возвращает строки, содержащие какой ход можно сделатье
        {
            //перербираем все клетки
            foreach (Square square in Square.YieldBoardSquares())
            {
                //если цвет фигуры равен цвету чей сейчас ход то возврат названия фигуры и имя клетки
                if (GetFigureAt(square).GetColor() == moveColor)
                {
                    //возврат клетки+какая там фигура
                    yield return new FigureOnSquare(GetFigureAt(square), square); //возврат каждого элемента(клетка+фигура) по одному
                }

            }
        }

        public Figure GetFigureAt(Square square) //получает фигуру на указ. клетке 
        {
            if (square.OnBoard()) //если клетка есть на доске
            {
                return figures[square.x, square.y];
            }
            else return Figure.none;
        }
        private void Init() //инииализация
        {
            // rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            // 0                                           1 2    3 4 5
            
            string[] parts = fen.Split(); //разбиение fen на 6 частей
            InitFigures(parts[0]); //иниц-ия расстановки фигур
            InitMoveColor(parts[1]); //иниц-ия цвета хода
            InitCastleFlags(parts[2]); //иниц-ия ракировки
            InitEnpassant(parts[3]); //иниц-ия битова поля
            InitDrawNumber(parts[4]); //иниц-ия кол-ва ходов для проверки правила 50 ходов
            InitMoveNumber(parts[5]); //иниц-ия кол-ва сделанных ходов
        }

        public bool IsCheckAfter(FigureMoving fm) //проверка на шаг после хода
        {
            Board after = Move(fm); //делаем ход
            return after.CanEatKing(); //проверка на шаг
        }

        private bool CanEatKing() //можно ли сьесть короля
        {
            Square badKing = FindBadKing(); //находим короля противника
            //пытаемся каждой фигурой съесть короля
            Moves moves = new Moves(this); //на основе тек.экземпляра создаём экз.класса ходов
            foreach(FigureOnSquare fs in YieldMyFigureOnSquares()) //перебираем только наши фигуры на клетках
            {
                if(moves.CanMove(new FigureMoving(fs,badKing))) //проверяем может ли тек.фигура пойти на клетку противника короля
                {
                    return true; //значит король может быть съеден
                }
            }
            return false;
        }

        public bool IsCheck() //проверка на шаг
        {
            return IsCheckAfter(FigureMoving.none); //не делаем хода 
        }

        Square FindBadKing() //находим короля противника
        {
            Figure king = moveColor == Color.white ? Figure.blackKing : Figure.whiteKing;
            foreach(Square square in Square.YieldBoardSquares()) //перебираем все клетки
            {
                if (GetFigureAt(square) == king) return square; //находим короля противника
            }
            return Square.none; //возврат пустой если не нашли
        }

        private void InitFigures(string v)
        {
            // 8 -> 71 -> 611 -> 51111 -> ...
            for (int j = 8; j >= 2; j--) //вместо 8 будет 11111111
                v = v.Replace(j.ToString(), (j - 1).ToString() + "1");
            v = v.Replace('1', (char)Figure.none); //присваиваем пустым клеткам значение none
            string[] lines = v.Split('/'); //разделяем на подстроки до /
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    figures[x, y] = (Figure)lines[7 - y][x]; //преобразуем команду fen в фигуру
        }

        private void InitMoveColor(string v)
        {
            //если некорректно задано то будет всегда белое
            moveColor = (v == "b") ? Color.black : Color.white; //определение цвета хода по букве команды
        }

        private void InitCastleFlags(string v)
        {
            //в зависимости от команды инициализируем возожность ракировки
            canCastleA1 = v.Contains("Q");
            canCastleH1 = v.Contains("K");
            canCastleA8 = v.Contains("q");
            canCastleH8 = v.Contains("k");
        }

        private void InitEnpassant(string v)
        {
            enpassant = new Square(v); //в зависимости от команды создаём битово поле, иначе пустая клетка 
        }

        private void InitDrawNumber(string v)
        {
            drawNumber = int.Parse(v);
        }

        private void InitMoveNumber(string v)
        {
            moveNumber = int.Parse(v);
        }
    }  
}
