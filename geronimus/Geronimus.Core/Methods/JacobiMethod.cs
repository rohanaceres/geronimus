﻿using Geronimus.Core.Model;
using Geronimus.Core.Extensions;

namespace Geronimus.Core.Methods
{
    /// <summary>
    /// Jacobi method to solve systems of linear equations.
    /// </summary>
    public sealed class JacobiMethod : AbstractMethod
    {
        /// <summary>
        /// Solve the <see cref="System"/>.
        /// </summary>
        /// <returns><see cref="System"/> result.</returns>
        /// <exception cref="DivideByZeroException">If a division by zero occurred.</exception>
        protected override LinearSystemResult SolveItByConcrete()
        {
            LinearSystemResult result = new LinearSystemResult();

            double [] r = new double [this.Dimension.Value];
            double [] tmp = new double[this.Dimension.Value];

            do
            {
                r.CopyTo(tmp, 0);

                for (int i = this.Dimension.Value - 1; i >= 0; i--)
                {
                    r[i] = this.System.Equations[i].Variables[this.Dimension.Value];

                    for (int j = 0; j < this.Dimension.Value; j++)
                    {
                        if (j != i)
                        {
                            r[i] -= this.System.Equations[i].Variables[j] * tmp[j];
                        }
                    }

                    r[i] /= this.System.Equations[i].Variables[i];
                }

                result.Iterations++;
            }
            while (ShouldIStay(r, tmp) == true);

            result.X = r[0];
            result.Y = r[1];
            result.Z = r[2];

            return result;
        }
        

        /// <summary>
        /// Verify if the algorithm should stop or continue iterating.
        /// Return true if should stay.
        /// </summary>
        /// <returns>If the algorithm should stop or continue iterating.</returns>
        private bool ShouldIStay(double [] a, double [] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if ((a[i] - b[i]).InvertIfNegative() > this.ErrorRate)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
