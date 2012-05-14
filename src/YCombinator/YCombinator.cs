using System;

namespace YCombinator
{
    public static class YCombinator
    {
        /// <summary>
        /// Takes a single parameter, anonymous function that can recur on itself
        /// </summary>
        /// <param name="func">Anonymous function</param>
        /// <returns>Results of the anonymous function</returns>
        public static dynamic Y( Func<dynamic, Func<dynamic, dynamic>> func )
        {
            Func<dynamic, Func<dynamic, dynamic>> recur =
                x => y => func( x( x ) )( y );

            return recur( recur );
        }

        /// <summary>
        /// Takes an anonymous function with 2 parameters that can recur on itself
        /// </summary>
        /// <param name="func">Anonymous function</param>
        /// <returns>Results of the anonymous function</returns>
        public static dynamic Y( Func<dynamic, Func<dynamic, dynamic, dynamic>> func )
        {
            Func<dynamic, Func<dynamic, dynamic, dynamic>> recur =
                x => ( y, y2 ) => func( x( x ) )( y, y2 );

            return recur( recur );
        }

        public static void Iterations()
        {
            // first iteration - basic recursive method
            var result = Factorial( 6 );
            Output( 1, result );

            // second iteration - recursive Func
            Func<dynamic, dynamic> factorial2 = null;
            factorial2 = ( x ) =>
            {
                if ( x < 2 )
                    return 1;

                return x * factorial2( --x );
            };

            result = factorial2( 6 );
            Output( 2, result );

            // third iteration - Func takes function that will be called recursivly on itself
            Func<dynamic, dynamic, dynamic> factorial3 = ( func, x ) =>
            {
                if ( x < 2 )
                    return 1;

                return x * func( func, --x );
            };

            result = factorial3( factorial3, 6 );
            Output( 3, result );

            // fourth iteration - currying the func so we dont have to pass it as a parameter
            Func<dynamic, Func<dynamic, dynamic>> factorial4 = ( func ) => ( x ) =>
            {
                if ( x < 2 )
                    return 1;

                return x * ( func( func ) )( --x );
            };

            result = factorial4( factorial4 )( 6 );
            Output( 4, result );

            // fifth iteration - this will extract a layer of recursion into a separate function
            Func<dynamic, Func<dynamic, dynamic>> factorial5 = ( func ) => ( x ) =>
            {
                if ( x < 2 )
                    return 1;

                return x * func( --x );
            };
            Func<dynamic, Func<dynamic, dynamic>> recur5 = null;
            recur5 = ( rec ) => ( x ) =>
            {
                return factorial5( recur5( recur5 ) )( x );
            };

            result = recur5( recur5 )( 6 );
            Output( 5, result );

            // sixth iteration - create a wrapper function
            // we now have a function that takes an anonymous function that will recur on itself.
            // we are basically able to create any recursive function on the fly that will take any parameter
            Func<Func<dynamic, Func<dynamic, dynamic>>, dynamic> recurWrapper = ( f ) =>
            {
                Func<dynamic, Func<dynamic, dynamic>> recur = ( x ) => ( y ) => f( x( x ) )( y );

                return recur( recur );
            };

            var factorial6 = recurWrapper( ( func ) => ( x ) =>
            {
                if ( x < 2 )
                    return 1;

                return x * func( --x );
            } );

            var reverseString6 = recurWrapper( ( func ) => ( x ) =>
            {
                string str = x;

                if ( str.Length < 2 )
                    return str;

                return str[str.Length - 1] + func( str.Substring( 0, str.Length - 1 ) );
            } );

            result = factorial6( 6 );
            Output( 6, result );
            Output( 6, reverseString6( "Recursion!" ) );
        }

        public static int Factorial( int num )
        {
            if ( num < 2 )
                return 1;

            return num * Factorial( --num );
        }

        private static void Output( int iteration, dynamic result )
        {
            Console.WriteLine( string.Format( "{0} - {1}\n", iteration, result ) );
        }
    }
}
