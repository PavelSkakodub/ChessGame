using System;

namespace ChessRules
{
    class FigureMoving //класс перемещения фигуры
    {
        public Figure figure { get; private set; } //фигура
        public Square from { get; private set; } //откуда перемещается
        public Square to { get; private set; } //куда перемещается
        public Figure promotion { get; private set; } //превращение пешки если дошла до конца

        public static FigureMoving none = new FigureMoving(); //пустой экземпляр

        public FigureMoving(FigureOnSquare fs,Square to,Figure promotion = Figure.none) //задание перемещения и превращение по умолчанию нету
        {
            figure = fs.figure;
            from = fs.square;
            this.to = to;
            this.promotion = promotion;
        }

        FigureMoving()
        {
            //отсутствие хода
            figure = Figure.none;
            from = Square.none;
            to = Square.none;
            promotion = Figure.none;

        }

        public FigureMoving(string move) //Pe2e4 - пешка ходит на 2 вперёд, Pe7e8Q - пешка ходит вперёд и превращается в ферзя
        {
            figure = (Figure)move[0]; //приводим 1 символ к типу фигура (буква обознач-ая фигуру)
            from = new Square(move.Substring(1, 2)); //с 1 индекса берём 2 символа
            to = new Square(move.Substring(3, 2)); //с 3 индекса берём 2 символа
            if (move.Length == 6) //если появилась команда превращения(в конце всей команды)
            {
                promotion = (Figure)move[5]; //превращаем в указ-ую фигуру
            }
            else promotion = Figure.none; //нет превращения
        }

        public override string ToString() //переопределение метода конвертации в строку
        {
            return ((char)figure).ToString() + 
                from.Name + 
                to.Name + 
                (promotion == Figure.none ? "" : ((char)promotion).ToString()); //возврат фигры+откуда и куда+добавление буквы если есть превращение
        }

        public int DeltaX { get { return to.x - from.x; } } //смещение при ходе по х
        public int DeltaY { get { return to.y - from.y; } } //смещение при ходе по у

        public int AbsDeltaX { get { return Math.Abs(DeltaX); } } //модуль чтобы не было отрицательных чисел
        public int AbsDeltaY { get { return Math.Abs(DeltaY); } } //модуль чтобы не было отрицательных чисел
    
        public int SignX { get { return Math.Sign(DeltaX); } } //определяет в какую сторону смещение по х
        public int SignY { get { return Math.Sign(DeltaY); } } //определяет в какую сторону смещение по y
    
        public Figure PlacedFigure 
        { 
            get 
            { 
                return promotion == Figure.none ? figure : promotion; 
            } 
        }
    }
}
