using System.Text;

namespace ChessRules
{ 
    //этот класс расширяет базовый класс Board и делает его чище+при создании экземпляра этого класса сразу рисуется новая доска с сделанным ходом
    class NextBoard:Board //класс хода фигуры, наследуется от класса доски
    {
        FigureMoving fm;

        public NextBoard(string fen, FigureMoving fm):base(fen) //
        {
            this.fm = fm; //сохраняем текущий fm
            MoveFigures(); //делаем ход
            DropEnpassant(); //после взятия на проходе
            SetEnpassant(); //проверка было ли битово поле
            MoveCastlingRook(); //переместить ладью при ракировке
            UpdateCastleFlags(); //фиксируем ракировку
            MoveNumber(); //считаем кол-во всех ходов
            MoveColor(); //переключаем на ход противника
            GenerateFEN(); //генерация fen
        }
        private void DropEnpassant()
        {
            if(fm.to==enpassant) //если идём туда где битое поле
            {
                if (fm.figure == Figure.whitePawn || fm.figure == Figure.blackPawn)  //если фигура это пешка (только у них битое поле есть)
                {
                    SetFigureAt(new Square(fm.to.x, fm.from.y),Figure.none); //реагируем на битово поле
                }
            }

        }

        private void MoveCastlingRook() //перемещение ладьи после ракировки
        {
            if (fm.figure == Figure.whiteKing) //если ходил белый король
            {
                if (fm.from == new Square("e1")) //если пошёл с клетки е1
                {
                    if (fm.to == new Square("g1")) //если пошёл на клетку g1
                    {
                        SetFigureAt(new Square("h1"), Figure.none); //стираем прошлую фигуру (ладью)
                        SetFigureAt(new Square("f1"), Figure.whiteRook); //ставим ладью на новое место
                        return;
                    }
                }
            }
            if (fm.figure == Figure.whiteKing) //если ходил белый король
            {
                if (fm.from == new Square("e1")) //если пошёл с клетки е1
                {
                    if (fm.to == new Square("c1")) //если пошёл на клетку c1
                    {
                        SetFigureAt(new Square("a1"), Figure.none); //стираем прошлую фигуру (ладью)
                        SetFigureAt(new Square("d1"), Figure.whiteRook); //ставим ладью на новое место
                        return;
                    }
                }
            }

            if (fm.figure == Figure.blackKing) //если ходил чёрный король
            {
                if (fm.from == new Square("e8")) //если пошёл с клетки е8
                {
                    if (fm.to == new Square("g8")) //если пошёл на клетку g8
                    {
                        SetFigureAt(new Square("h8"), Figure.none); //стираем прошлую фигуру (ладью)
                        SetFigureAt(new Square("f8"), Figure.blackRook); //ставим ладью на новое место
                        return;
                    }
                }
            }
            if (fm.figure == Figure.blackKing) //если ходил чёрный король
            {
                if (fm.from == new Square("e8")) //если пошёл с клетки е8
                {
                    if (fm.to == new Square("c8")) //если пошёл на клетку c8
                    {
                        SetFigureAt(new Square("a8"), Figure.none); //стираем прошлую фигуру (ладью)
                        SetFigureAt(new Square("d8"), Figure.blackRook); //ставим ладью на новое место
                        return;
                    }
                }
            }
        }

        private void MoveNumber()
        {
            if (moveColor == Color.black) //если был ход чёрных то увеличиваем кол-во ходов
            {
                moveNumber++;
            }
        }
        private void MoveColor()
        {
            moveColor = moveColor.FlipColor(); //переключаем ход
        }

        private void MoveFigures() //ход фигуры
        {
            SetFigureAt(fm.from, Figure.none); //подняли фигуру (клетка стала пустой)
            SetFigureAt(fm.to, fm.PlacedFigure); //ставим фигуру куда она пошла и превращаем если задано превращение и битово поле (для пешки)
        }

        private void SetEnpassant()
        {
            enpassant = Square.none;
            if (fm.figure == Figure.whitePawn) 
            {
                if (fm.from.y == 1 && fm.to.y == 3) //если такое смешение значит появилось битово поле
                {
                    enpassant = new Square(fm.from.x, 2);
                }
            }
            if (fm.figure == Figure.blackPawn)
            {
                if (fm.from.y == 6 && fm.to.y == 4) //если такое смешение значит появилось битово поле
                {
                    enpassant = new Square(fm.from.x, 5);
                }
            }
        }

        private void SetFigureAt(Square square, Figure figure) //установка фигуры в клетку
        {
            if (square.OnBoard()) //если клетка на доске
            {
                figures[square.x, square.y] = figure;
            }
        }

        private void UpdateCastleFlags() //ракировка
        {
            switch(fm.figure) //узнаём кто ходил
            {
                case Figure.whiteKing: //если ходил король значит ракировки быть не может
                    canCastleA1 = false;
                    canCastleH1 = false;
                    return;
                case Figure.whiteRook: //в зависимости от хода ладьи убирается ракировка
                    if (fm.from == new Square("a1")) 
                        canCastleA1 = false;
                    if (fm.from == new Square("h1")) 
                        canCastleH1 = false;
                    return;
                case Figure.blackKing: //если ходил король значит ракировки быть не может
                    canCastleA8 = false;
                    canCastleH8 = false;
                    return;
                case Figure.blackRook: //в зависимости от хода ладьи убирается ракировка
                    if (fm.from == new Square("a8"))
                        canCastleA8 = false;
                    if (fm.from == new Square("h8"))
                        canCastleH8 = false;
                    return;
                default: return;
            }
        }

        private void GenerateFEN() //генерация нового fen на основе сделанного хода
        {
            fen = FenFigures() + " " +
                  FenMoveColor() + " " +
                  FenCastleFlags() + " " +
                  FenEnpassant() + " " +
                  FenDrawNumber() + " " +
                  FenMoveNumber();
        }

        private string FenFigures()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    sb.Append(figures[x, y] == Figure.none ? '1' : //проерка на пустую фигуру (пишем 1 в fen) 
                    (char)figures[x, y]); //добавляем фигуру в строку fen команды
                }
                if (y > 0)  //переход на следующую строку fen команды
                {
                    sb.Append("/");
                }
            }
            string eight = "11111111";
            for (int j = 8; j >= 2; j--) //меняем 8 единиц на 8, 7 единиц на 7 и т.д для упрощения fen команды 
            {
                sb = sb.Replace(eight.Substring(0, j), j.ToString());
            }
            return sb.ToString();
        }

        private string FenMoveColor()
        {
            return moveColor == Color.white ? "w" : "b"; //если ходят белые то возврат w иначе b
        }

        private string FenCastleFlags()
        {
            //ракировка в fen команду
            string flags =
                 (canCastleA1 ? "Q" : "") +
                 (canCastleH1 ? "K" : "") +
                 (canCastleA8 ? "q" : "") +
                 (canCastleH8 ? "k" : "");
            return flags == "" ? "-" : flags; //в случае невозможности ракировки
        }

        private string FenEnpassant()
        {
            return enpassant.Name; //возврат названия клетки (если нету то -)
        }

        private string FenDrawNumber()
        {
            return drawNumber.ToString(); //конвертим в строку для fen
        }

        private string FenMoveNumber()
        {
            return moveNumber.ToString(); //конвертим в строку для fen
        }

    }
}
