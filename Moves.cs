
using System;

namespace ChessRules
{
    class Moves
    {
        FigureMoving fm; //кто ходит
        Board board; //для проверки

        public Moves(Board board)
        {
            this.board = board;
        }
        public bool CanMove(FigureMoving fm) //проверяет можно ли сделать ход
        {
            this.fm = fm;
            return CanMoveFrom() && CanMoveTo() && CanFigureMove();
        }
        private bool CanFigureMove() //ход фигуры
        {
            switch (fm.figure)
            {
                case Figure.whiteKing:
                case Figure.blackKing:
                    return CanKingMove() || CanKingCastle();//проверка ходов короля

                case Figure.whiteQueen:
                case Figure.blackQueen:
                    return CanStraightMove();

                case Figure.whiteRook:
                case Figure.blackRook:
                    //нужно убедиться что ладья движется прямо(смещение по х или у == 0)
                    return (fm.SignX == 0 || fm.SignY == 0)&&CanStraightMove();

                case Figure.whiteBishop:
                case Figure.blackBishop:
                    //нужно убедиться что слон движется по диагонали (смещение по х и у != 0)
                    return (fm.SignX != 0 && fm.SignY != 0) && CanStraightMove();

                case Figure.whiteKnight:
                case Figure.blackKnight:
                    return CanKnightMove(); //проверка ходов коня

                case Figure.whitePawn:
                case Figure.blackPawn:
                    return CanPawnMove(); //проверка ходов пешки

                default: 
                    return false;
            }
        }
        private bool CanMoveTo()
        {
            //если находится на доске, белая фигура не может пойти на клетку с белой фигурой, либо пустую либо на черных
            return fm.to.OnBoard() && board.GetFigureAt(fm.to).GetColor()!= board.moveColor;
        }
        private bool CanMoveFrom()
        {
            //если есть на доске и если цвет совпадает с цветом хода
            return fm.from.OnBoard() && fm.figure.GetColor() == board.moveColor;
        }
        bool CanKingMove() //ход короля
        {
            return (fm.AbsDeltaX <= 1) && (fm.AbsDeltaY <= 1); //т.е ход на 1 клетку или на месте
        }
        bool CanKingCastle()
        {
            //для белого короля
            if (fm.figure == Figure.whiteKing) //если фигура это белый король
            {
                //для правой ракировки
                if (fm.from == new Square("e1")) //если идёт с клетки е1
                {
                    if (fm.to == new Square("g1")) //если идёт на клетку g1
                    {
                        if (board.canCastleH1) //если ракируется в королевскую сторону 
                        {
                            if (board.GetFigureAt(new Square("h1")) == Figure.whiteRook) //если на h1 стоит ладья
                            {
                                if (board.GetFigureAt(new Square("f1")) == Figure.none) //если между королём и ладьей нет фигур
                                {
                                    if (board.GetFigureAt(new Square("g1")) == Figure.none) //если между королём и ладьей нет фигур
                                    {
                                        if(!board.IsCheck()) //проверка на шаг
                                        if(!board.IsCheckAfter(new FigureMoving("Ke1f1"))) //проверка на шаг после указанного хода
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }

                //для левой ракировки
                if (fm.from == new Square("e1")) //если идёт с клетки е1
                {
                    if (fm.to == new Square("c1")) //если идёт на клетку c1
                    {
                        if (board.canCastleA1) //если ракируется в королевскую сторону 
                        {
                            if (board.GetFigureAt(new Square("a1")) == Figure.whiteRook) //если на a1 стоит ладья
                            {
                                if (board.GetFigureAt(new Square("b1")) == Figure.none) //если между королём и ладьей нет фигур
                                {
                                    if (board.GetFigureAt(new Square("c1")) == Figure.none) //если между королём и ладьей нет фигур
                                    {
                                        if (board.GetFigureAt(new Square("d1")) == Figure.none)
                                        {
                                            if(!board.IsCheck()) //проверка на шаг
                                            if(!board.IsCheckAfter(new FigureMoving("Ke1d1"))) //проверка на шаг после указанного хода
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //для чёрного короля
            if (fm.figure == Figure.blackKing) //если фигура это чёрный король
            {
                //для правой ракировки
                if (fm.from == new Square("e8")) //если идёт с клетки е8
                {
                    if (fm.to == new Square("g8")) //если идёт на клетку g8
                    {
                        if (board.canCastleH8) //если ракируется в королевскую сторону 
                        {
                            if (board.GetFigureAt(new Square("h8")) == Figure.blackRook) //если на h8 стоит ладья
                            {
                                if (board.GetFigureAt(new Square("f8")) == Figure.none) //если между королём и ладьей нет фигур
                                {
                                    if (board.GetFigureAt(new Square("g8")) == Figure.none) //если между королём и ладьей нет фигур
                                    {
                                        if(!board.IsCheck()) //проверка на шаг
                                        if(!board.IsCheckAfter(new FigureMoving("ke8f8"))) //проверка на шаг после указанного хода
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }

                //для левой ракировки
                if (fm.from == new Square("e8")) //если идёт с клетки е8
                {
                    if (fm.to == new Square("c8")) //если идёт на клетку c8
                    {
                        if (board.canCastleA8) //если ракируется в королевскую сторону 
                        {
                            if (board.GetFigureAt(new Square("a8")) == Figure.blackRook) //если на a8 стоит ладья
                            {
                                if (board.GetFigureAt(new Square("b8")) == Figure.none) //если между королём и ладьей нет фигур
                                {
                                    if (board.GetFigureAt(new Square("c8")) == Figure.none) //если между королём и ладьей нет фигур
                                    {
                                        if (board.GetFigureAt(new Square("d8")) == Figure.none)
                                        {
                                            if(!board.IsCheck()) //проверка на шаг
                                            if(!board.IsCheckAfter(new FigureMoving("ke8d8"))) //проверка на шаг после указанного хода
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        } //ракировка короля
        bool CanKnightMove() //ход коня
        {
            //смещение буквой Г по х или по у
            return fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2 ||
                fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1;
        }
        bool CanStraightMove() //ход ферзя (по 1 клетке смещается)
        {
            Square at = fm.from; //клетка откуда идём (нач.позиция)
            do //фигура сразу делает ход поэтому сперва смещаем фигуру (нужно если вдруг пройдёт мимо фигуры и чтоб не ушла за доску)
            {
                at = new Square(at.x + fm.SignX, at.y + fm.SignY);
                if (at == fm.to) //если поймали фигуру
                    return true;

              //если мы двигаемся по доске и по пустым клеткам(где нет фигуры)
            } while (at.OnBoard() && board.GetFigureAt(at) == Figure.none);
            return false;
        }
        bool CanPawnMove() //ход пешки
        {
            if (fm.from.y < 1 || fm.from.y > 6) return false; //пешка не может быть на первом и последнем ряду (из-за превращения и то что обратно не ходит)
            int stepY = fm.figure.GetColor() == Color.white ? +1 : -1; //смещение по У (полож. или отриц. в зависимости от цвета пешки)
            return  CanPawnGo(stepY) || //может ли идти пешка
                    CanPawnJump(stepY) || //может ли прыгать(на 1 или 2 клетки)
                    CanPawnEat(stepY)|| //может ли пешка есть
                    CanPawnEnpassant(stepY); //обработка на битово поле
        }
        bool CanPawnGo(int stepY) //когда пешка может идти
        {
            if (board.GetFigureAt(fm.to) == Figure.none) //сначала проверяем что пешка идёт на пустую клетку
            {
                if (fm.DeltaX == 0)  //если идёт именно прямо (вертикально)
                {
                    if (fm.DeltaY == stepY)  //смещение ровно +1 т.е на 1 клетку идёт
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool CanPawnJump(int stepY) //когда пешка может прыгнуть через клетку
        {
            if (board.GetFigureAt(fm.to) == Figure.none) //сначала проверяем что пешка идёт на пустую клетку
            {
                if (fm.from.y == 1 && stepY == +1 || fm.from.y == 6 && stepY == -1) //белая пешка должна идти с 1 ряда, черная с 6 ряда 
                {
                    if (fm.DeltaX == 0) //должен двигаться прямо (вертикально)
                    {
                        if (fm.DeltaY == 2 * stepY) //смещение с знаком для цвета фигуры (на 2 клетки)
                        {
                            if (board.GetFigureAt(new Square(
                                fm.from.x, fm.from.y + stepY)) == Figure.none) //клетка через которую перепыгиваем должна быть пустой
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        bool CanPawnEat(int stepY) //когда пешка может съесть фигуру
        {
            if (board.GetFigureAt(fm.to) != Figure.none)  //куда идёт пешка там должна быть фигура
            {
                if (fm.AbsDeltaX == 1) //если можно на 1 клетку идти вправо или влево 
                {
                    if (fm.DeltaY == stepY) //если можно на 1 клетку вперёд (чтобы было по диагонали)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool CanPawnEnpassant(int stepY)
        {
            if (fm.to == board.enpassant) //клетка куда идём должна быть битым полем
            {
                if (board.GetFigureAt(fm.to) == Figure.none) //клетка ккуда идём должна быть пустой 
                {
                    if (fm.DeltaY == stepY) //двигаемся для чёрных вниз для белых вверх
                    {
                        if (fm.AbsDeltaX == 1)  //если сдвинулись вбок на 1 клетку (было взятие)
                        {
                            if (stepY == +1 && fm.from.y == 4 || stepY == -1 && fm.from.y == 3)  //если белые то бить с 4 горизонтали, у чёрных по-другому 
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        } //когда идёт взятие на проходе
    }
}
