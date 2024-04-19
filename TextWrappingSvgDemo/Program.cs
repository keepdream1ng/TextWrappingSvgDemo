using System.Drawing;
using Svg;
using Svg.Pathing;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: SvgCreator.exe <text>");
            return;
        }

        string text = args[0];

        SvgDocument svgDoc = new SvgDocument();
        svgDoc.Width = new SvgUnit(SvgUnitType.Pixel, 300);
        svgDoc.Height = new SvgUnit(SvgUnitType.Pixel, 150);

        // Just a dummy, for some positioning demonstration.
        SvgRectangle square = new SvgRectangle
        {
            Width = new SvgUnit(SvgUnitType.Pixel, 150),
            Height = new SvgUnit(SvgUnitType.Pixel, 150),
            X = new SvgUnit(SvgUnitType.Pixel, 0),
            Fill = new SvgColourServer(System.Drawing.Color.Blue)
        };
        svgDoc.Children.Add(square);


        // Path along which our text will go.
        SvgPath path = new SvgPath
        {
            ID = "path1",
            PathData = new Svg.Pathing.SvgPathSegmentList()
        };
        path.PathData.Add(new SvgMoveToSegment(false, new PointF(160, 20)));
        path.PathData.Add(new SvgLineSegment(false, new PointF(290, 20)));
        path.PathData.Add(new SvgMoveToSegment(false, new PointF(160, 60)));
        path.PathData.Add(new SvgLineSegment(false, new PointF(290, 60)));
        path.PathData.Add(new SvgMoveToSegment(false, new PointF(160, 100)));
        path.PathData.Add(new SvgLineSegment(false, new PointF(290, 100)));
        path.PathData.Add(new SvgMoveToSegment(false, new PointF(160, 140)));
        path.PathData.Add(new SvgLineSegment(false, new PointF(290, 140)));

        SvgDefinitionList definitionList = new SvgDefinitionList();
        svgDoc.Children.Add(definitionList);
        definitionList.Children.Add(path);

        SvgText svgText = new SvgText
        {
            FontSize = 10,
            Fill = new SvgColourServer(System.Drawing.Color.Black),
        };
        svgDoc.Children.Add(svgText);

        // A textPath element with a reference to set path's ID, which it will follow.
        SvgTextPath textPath = new SvgTextPath
        {
            StartOffset = new SvgUnit(SvgUnitType.Percentage, 0),
            ReferencedPath = new Uri($"#{path.ID}", UriKind.Relative),
            Text = text,
            TextAnchor = SvgTextAnchor.Start
        };
        svgText.Children.Add(textPath);

        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output.svg");
        svgDoc.Write(outputPath);

        Console.WriteLine($"SVG file saved to: {Path.GetFullPath(outputPath)}");
    }
    // Output example.
    /*
        <?xml version="1.0" encoding="utf-8"?>
        <!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">
        <svg version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:xml="http://www.w3.org/XML/1998/namespace" width="300px" height="150px">
          <rect x="0px" width="150px" height="150px" style="fill:blue;" />
          <defs>
            <path d="M160 20 L290 20 M160 60 L290 60 M160 100 L290 100 M160 140 L290 140" id="path1" />
          </defs>
          <text font-size="10" style="fill:black;">
            <textPath startOffset="0%" xlink:href="#path1" text-anchor="start">testing loooooooong line here, how will it fit? should it take next line maybe? Visual studio has a word wrap for arguments</textPath>
          </text>
        </svg>
     */
}