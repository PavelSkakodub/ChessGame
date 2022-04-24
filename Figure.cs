using System.Collections.Generic;
namespace ChessRules
{
    enum Figure //перечисление фигур
    {
        none,
        //белые фигуры
        whiteKing = 'K', //король
        whiteQueen = 'Q', //ферзь
        whiteRook = 'R', //ладья
        whiteBishop = 'B', //офицер
        whiteKnight = 'N', //конь
        whitePawn = 'P', //пешка
        //черные фигуры
        blackKing = 'k', //король
        blackQueen = 'q', //ферзь
        blackRook = 'r', //ладья
        blackBishop = 'b', //офицер
        blackKnight = 'n', //конь
        blackPawn = 'p', //пешка
    }

    static class FigureMethods
    {
        public static Color GetColor(this Figure figure) //метод расширения для класса фигуры
        {
            if (figure == Figure.none) //если никакая фигура - то и цвет никакой
            {
                return Color.none; 
            }

            //если ниж.регистр равен самому регистру то меняем цвет на черный иначе на белый
            //так как у белых регистр верхний, а у черных нижний
            return ((char)figure).ToString().ToLower() == ((char)figure).ToString() ? Color.black : Color.white;
        }

        public static IEnumerable<Figure> YieldPromotions(this Figure figure,Square to) //возвращает фигуры в которые можно превратится
        {
            switch (figure) //если фигура это белая пешка и стоит в последнем ряду
            {
                case Figure.whitePawn when to.y == 7:
                    //возвращает сначала первую фигуру, после 2 вызова этой функции будет вторая фигура и т.д
                    yield return Figure.whiteQueen; //при вызове 1 раза функции
                    yield return Figure.whiteRook; // при вызове 2 раза функции
                    yield return Figure.whiteBishop; //при вызове 3 раза функции
                    yield return Figure.whiteKnight; //при вызове 4 раза функции
                    break;
                case Figure.blackPawn when to.y == 0:
                    //возвращает сначала первую фигуру, после 2 вызова этой функции будет вторая фигура и т.д
                    yield return Figure.blackQueen; //при вызове 1 раза функции
                    yield return Figure.blackRook; // при вызове 2 раза функции
                    yield return Figure.blackBishop; //при вызове 3 раза функции
                    yield return Figure.blackKnight; //при вызове 4 раза функции
                    break;
                default:
                    yield return Figure.none; //иначе фигура не превращается
                    break;
            } //иначе фигура не превращается
        }
    }
}
