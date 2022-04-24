
namespace ChessRules
{
    class FigureOnSquare //класс контейнер, хранящий клетку и фигуру на ней
    {
        public Figure figure { get; private set; }
        public Square square { get; private set; }

        public FigureOnSquare(Figure figure,Square square)
        {
            this.figure = figure;
            this.square = square;
        }
    }
}
