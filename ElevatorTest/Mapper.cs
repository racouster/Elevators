// See https://aka.ms/new-console-template for more information
public static class Mapper
{
    private static readonly string _map = @"1                                                                                    .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                     .
                                                                                   99.";
    private static char[] ParseMap(string map)
    {
        var mapArray = map?.ToCharArray() ?? _map.ToCharArray();
        return mapArray;
    }
}
