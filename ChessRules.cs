using System.Collections.Generic;

namespace ChessRules
{
    public class Chess
    {
        public string fen //получает актуальное размещение из класса доски
        {
            get
            {
                return board.fen; 
            }
        } 
        public bool IsCheck { get; private set; } //наличие шаха
        public bool IsCheckmate { get; private set; } //наличие мата
        public bool IsStalemate { get; private set; } //наличие пата

        Board board;
        Moves moves;
        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") //нотации Форсиса-Эдварда
        {
            //rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR это размещение фигур; 
            //w-чей ход; KQkq - признаки ракировки; - это признаки взятия на проходе;
            //0 - ск-ко ходов по правилам 50 ходов; 1- номер тек-го хода 
            board = new Board(fen);
            moves = new Moves(board); //алгоритмы проверки ходов
            SetCheckFlags(); 
        }

        private Chess(Board board) //конструктор с новой доской
        {
            this.board = board;
            moves = new Moves(board);
            SetCheckFlags(); //чтобы каждый раз обновлялось состояние игры
        }

        private void SetCheckFlags() //флаги установки состояний
        {
            IsCheck = board.IsCheck(); //установка шага если он был
            IsCheckmate = false; //мата не было пока
            IsStalemate = false; //пата не было пока
            foreach(string moves in YieldValidMoves())
                return; //если есть хоть один ход то мата,пата нету
            if (IsCheck)
                IsCheckmate = true; //при отсутствии ходов и наличии шага получаетя мат
            else
                IsStalemate = true; //иначе получился пат
        }

        public bool IsValidMove(string move)
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm)) //если нельзя сделать ход
            {
                return false; //то доска не изменяется
            }
            if (board.IsCheckAfter(fm)) //если будет шаг после этого хода
            {
                return false; //то ходить так нельзя
            }
            return true;
        }

        public Chess Move(string move) //выполняет ход
        {
            if (!IsValidMove(move)) return this;
            FigureMoving fm = new FigureMoving(move);
            Board nextBoard = board.Move(fm); //создаём новую доску с дел-ым ходом
            Chess nextChess = new Chess(nextBoard); //создаём новые шахматы на основе этой доски
            return nextChess;
        }

        public char GetFigureAt(int x,int y) //получает размещение фигур из координат
        {
            Square square = new Square(x, y);
            Figure figure = board.GetFigureAt(square); //получаем фигуру на указ.клетке
            return figure == Figure.none ? '.' : (char)figure; //если фигуры нет то возврат точки, иначе конвертируем в символы
        }

        public char GetFigureAt(string xy) //получает размещение фигур из координат
        {
            Square square = new Square(xy);
            Figure figure = board.GetFigureAt(square); //получаем фигуру на указ.клетке
            return figure == Figure.none ? '.' : (char)figure; //если фигуры нет то возврат точки, иначе конвертируем в символы
        }

        public IEnumerable<string> YieldValidMoves() //возвращает строки, содержащие какой ход можно сделатье
        {
            foreach(FigureOnSquare fs in board.YieldMyFigureOnSquares()) //перербираем все клетки с фигурой внутри
            {
                foreach(Square to in Square.YieldBoardSquares()) //перебирает клетки куда мы можем пойти
                {
                    foreach(Figure promotion in fs.figure.YieldPromotions(to)) //перебирает и предлагает 4 варианта превращения пешки или без превращения
                    {
                        FigureMoving fm = new FigureMoving(fs, to, promotion); //задание перемещения
                        if (moves.CanMove(fm)) //возврат только если можно туда пойти
                        {
                            if(!board.IsCheckAfter(fm)) //если нету шага после хода
                            yield return fm.ToString(); //делает возврат по одному
                        }
                    }
                }
            }
        }
    }
}
