using System;

namespace YCombinator
{
    class Program
    {
        static void Main( string[] args )
        {
            // steps to get to the final method
            YCombinator.Iterations();

            // seventh iteration - create method that will take an anonymous function and recur on itself
            var factorial = YCombinator.Y( func => x =>
                x <= 2 ? x : x * func( --x ) );

            var reverseString = YCombinator.Y( func => x =>
            {
                string str = x;

                if ( str.Length < 2 )
                    return str;

                return str[str.Length - 1] + func( str.Substring( 0, str.Length - 1 ) );
            } );

            Console.WriteLine( factorial( 6 ) );
            Console.WriteLine( factorial( 10 ) );
            Console.WriteLine( reverseString( "YCombinator!" ) );
            Console.WriteLine();

            // 2 parameter function
            var subtraction = YCombinator.Y( func => ( x, y ) =>
                                     {
                                         if ( x < y ) return -1 * func( y, x );
                                         if ( x <= y ) return 0;
                                         return 1 + func( --x, y );
                                     } );

            var addition = YCombinator.Y( func => ( x, y ) =>
                                    {
                                        if ( x == 0 && y == 0 ) return 0;
                                        if ( x == 0 ) return 1 + func( x, --y );
                                        return 1 + func( --x, y );
                                    } );
             
            Console.WriteLine( "24 - 6 = " + subtraction( 24, 6 ) );
            Console.WriteLine( "24 - 60 = " + subtraction( 24, 60 ) );
            Console.WriteLine( "24 + 6 = " + addition( 24, 6 ) );
        }
    }
}
