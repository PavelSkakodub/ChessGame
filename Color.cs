namespace ChessRules
{
    enum Color //цвет всех фигур
    {
        none,
        white,
        black
    }

    static class ColorMethods
    {
        //метод расширяющий класс
        public static Color FlipColor(this Color color) //переворот цвета после каждого хода
        {
            //изменяем цвет на противоположный иначе возвращает ничего
            if (color == Color.black) return Color.white;
            if (color == Color.white) return Color.black;
            return color;
        }
    }
}
