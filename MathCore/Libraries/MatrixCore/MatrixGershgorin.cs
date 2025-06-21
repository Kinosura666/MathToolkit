using MathCore.Models;
using MathCore.Models.MatrixResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Libraries.MatrixCore
{
    public class MatrixGershgorin
    {
        public static GershgorinDiscsResult GershgorinDiscs(MatrixModel model)
        {
            int n = model.Rows;
            int m = model.Columns;

            var discs = new Disc[n];
            double globalMin = double.PositiveInfinity;
            double globalMax = double.NegativeInfinity;

            for (int i = 0; i < n; i++)
            {
                double center = model.Data[i, i];
                double radius = 0;

                for (int j = 0; j < m; j++)
                {
                    if (i != j)
                        radius += Math.Abs(model.Data[i, j]);
                }

                discs[i] = new Disc { Center = center, Radius = radius };

                double left = center - radius;
                double right = center + radius;

                if (left < globalMin) globalMin = left;
                if (right > globalMax) globalMax = right;
            }

            return new GershgorinDiscsResult
            {
                Discs = discs,
                MinBound = globalMin,
                MaxBound = globalMax
            };
        }
    }
}
