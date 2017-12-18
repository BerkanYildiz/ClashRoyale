namespace ClashRoyale.Maths
{
    public static class Math
    {
        private static readonly byte[] SQRT_TABLE =
        {
            0x00, 0x10, 0x16, 0x1B, 0x20, 0x23, 0x27, 0x2A, 0x2D,
            0x30, 0x32, 0x35, 0x37, 0x39, 0x3B, 0x3D, 0x40, 0x41,
            0x43, 0x45, 0x47, 0x49, 0x4B, 0x4C, 0x4E, 0x50, 0x51,
            0x53, 0x54, 0x56, 0x57, 0x59, 0x5A, 0x5B, 0x5D, 0x5E,
            0x60, 0x61, 0x62, 0x63, 0x65, 0x66, 0x67, 0x68, 0x6A,
            0x6B, 0x6C, 0x6D, 0x6E, 0x70, 0x71, 0x72, 0x73, 0x74,
            0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D,
            0x7E, 0x80, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86,
            0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F,
            0x90, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x96,
            0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9B, 0x9C, 0x9D, 0x9E,
            0x9F, 0xA0, 0xA0, 0xA1, 0xA2, 0xA3, 0xA3, 0xA4, 0xA5,
            0xA6, 0xA7, 0xA7, 0xA8, 0xA9, 0xAA, 0xAA, 0xAB, 0xAC,
            0xAD, 0xAD, 0xAE, 0xAF, 0xB0, 0xB0, 0xB1, 0xB2, 0xB2,
            0xB3, 0xB4, 0xB5, 0xB5, 0xB6, 0xB7, 0xB7, 0xB8, 0xB9,
            0xB9, 0xBA, 0xBB, 0xBB, 0xBC, 0xBD, 0xBD, 0xBE, 0xBF,
            0xC0, 0xC0, 0xC1, 0xC1, 0xC2, 0xC3, 0xC3, 0xC4, 0xC5,
            0xC5, 0xC6, 0xC7, 0xC7, 0xC8, 0xC9, 0xC9, 0xCA, 0xCB,
            0xCB, 0xCC, 0xCC, 0xCD, 0xCE, 0xCE, 0xCF, 0xD0, 0xD0,
            0xD1, 0xD1, 0xD2, 0xD3, 0xD3, 0xD4, 0xD4, 0xD5, 0xD6,
            0xD6, 0xD7, 0xD7, 0xD8, 0xD9, 0xD9, 0xDA, 0xDA, 0xDB,
            0xDB, 0xDC, 0xDD, 0xDD, 0xDE, 0xDE, 0xDF, 0xE0, 0xE0,
            0xE1, 0xE1, 0xE2, 0xE2, 0xE3, 0xE3, 0xE4, 0xE5, 0xE5,
            0xE6, 0xE6, 0xE7, 0xE7, 0xE8, 0xE8, 0xE9, 0xEA, 0xEA,
            0xEB, 0xEB, 0xEC, 0xEC, 0xED, 0xED, 0xEE, 0xEE, 0xEF,
            0xF0, 0xF0, 0xF1, 0xF1, 0xF2, 0xF2, 0xF3, 0xF3, 0xF4,
            0xF4, 0xF5, 0xF5, 0xF6, 0xF6, 0xF7, 0xF7, 0xF8, 0xF8,
            0xF9, 0xF9, 0xFA, 0xFA, 0xFB, 0xFB, 0xFC, 0xFC, 0xFD,
            0xFD, 0xFE, 0xFE, 0xFF
        };

        private static readonly byte[] ATAN_TABLE =
        {
            0x00, 0x00, 0x01, 0x01, 0x02, 0x02, 0x03, 0x03, 0x04,
            0x04, 0x04, 0x05, 0x05, 0x06, 0x06, 0x07, 0x07, 0x08,
            0x08, 0x08, 0x09, 0x09, 0x0A, 0x0A, 0x0B, 0x0B, 0x0B,
            0x0C, 0x0C, 0x0D, 0x0D, 0x0E, 0x0E, 0x0E, 0x0F, 0x0F,
            0x10, 0x10, 0x11, 0x11, 0x11, 0x12, 0x12, 0x13, 0x13,
            0x13, 0x14, 0x14, 0x15, 0x15, 0x15, 0x16, 0x16, 0x16,
            0x17, 0x17, 0x18, 0x18, 0x18, 0x19, 0x19, 0x19, 0x1A,
            0x1A, 0x1B, 0x1B, 0x1B, 0x1C, 0x1C, 0x1C, 0x1D, 0x1D,
            0x1D, 0x1E, 0x1E, 0x1E, 0x1F, 0x1F, 0x1F, 0x20, 0x20,
            0x20, 0x21, 0x21, 0x21, 0x22, 0x22, 0x22, 0x23, 0x23,
            0x23, 0x23, 0x24, 0x24, 0x24, 0x25, 0x25, 0x25, 0x25,
            0x26, 0x26, 0x26, 0x27, 0x27, 0x27, 0x27, 0x28, 0x28,
            0x28, 0x28, 0x29, 0x29, 0x29, 0x29, 0x2A, 0x2A, 0x2A,
            0x2A, 0x2B, 0x2B, 0x2B, 0x2B, 0x2C, 0x2C, 0x2C, 0x2C,
            0x2D, 0x2D, 0x2D
        };

        private static readonly byte[] SIN_TABLE =
        {
            0x00, 0x10, 0x16, 0x1B, 0x20, 0x23, 0x27, 0x2A, 0x2D,
            0x30, 0x32, 0x35, 0x37, 0x39, 0x3B, 0x3D, 0x40, 0x41,
            0x43, 0x45, 0x47, 0x49, 0x4B, 0x4C, 0x4E, 0x50, 0x51,
            0x53, 0x54, 0x56, 0x57, 0x59, 0x5A, 0x5B, 0x5D, 0x5E,
            0x60, 0x61, 0x62, 0x63, 0x65, 0x66, 0x67, 0x68, 0x6A,
            0x6B, 0x6C, 0x6D, 0x6E, 0x70, 0x71, 0x72, 0x73, 0x74,
            0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D,
            0x7E, 0x80, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86,
            0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F,
            0x90, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x96,
            0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9B, 0x9C, 0x9D, 0x9E,
            0x9F, 0xA0, 0xA0, 0xA1, 0xA2, 0xA3, 0xA3, 0xA4, 0xA5,
            0xA6, 0xA7, 0xA7, 0xA8, 0xA9, 0xAA, 0xAA, 0xAB, 0xAC,
            0xAD, 0xAD, 0xAE, 0xAF, 0xB0, 0xB0, 0xB1, 0xB2, 0xB2,
            0xB3, 0xB4, 0xB5, 0xB5, 0xB6, 0xB7, 0xB7, 0xB8, 0xB9,
            0xB9, 0xBA, 0xBB, 0xBB, 0xBC, 0xBD, 0xBD, 0xBE, 0xBF,
            0xC0, 0xC0, 0xC1, 0xC1, 0xC2, 0xC3, 0xC3, 0xC4, 0xC5,
            0xC5, 0xC6, 0xC7, 0xC7, 0xC8, 0xC9, 0xC9, 0xCA, 0xCB,
            0xCB, 0xCC, 0xCC, 0xCD, 0xCE, 0xCE, 0xCF, 0xD0, 0xD0,
            0xD1, 0xD1, 0xD2, 0xD3, 0xD3, 0xD4, 0xD4, 0xD5, 0xD6,
            0xD6, 0xD7, 0xD7, 0xD8, 0xD9, 0xD9, 0xDA, 0xDA, 0xDB,
            0xDB, 0xDC, 0xDD, 0xDD, 0xDE, 0xDE, 0xDF, 0xE0, 0xE0,
            0xE1, 0xE1, 0xE2, 0xE2, 0xE3, 0xE3, 0xE4, 0xE5, 0xE5,
            0xE6, 0xE6, 0xE7, 0xE7, 0xE8, 0xE8, 0xE9, 0xEA, 0xEA,
            0xEB, 0xEB, 0xEC, 0xEC, 0xED, 0xED, 0xEE, 0xEE, 0xEF,
            0xF0, 0xF0, 0xF1, 0xF1, 0xF2, 0xF2, 0xF3, 0xF3, 0xF4,
            0xF4, 0xF5, 0xF5, 0xF6, 0xF6, 0xF7, 0xF7, 0xF8, 0xF8,
            0xF9, 0xF9, 0xFA, 0xFA, 0xFB, 0xFB, 0xFC, 0xFC, 0xFD,
            0xFD, 0xFE, 0xFE, 0xFF
        };

        public const int FIXED_SHIFT = 10;

        /// <summary>
        /// Returns the absolute value of valueA int value.
        /// </summary>
        public static int Abs(int Value)
        {
            if (Value < 0)
            {
                return -Value;
            }

            return Value;
        }
        
        /// <summary>
        /// Returns the trigonometric cosine of an angle.
        /// </summary>
        public static int Cos(int Angle)
        {
            return Math.Sin(Angle + 90);
        }

        /// <summary>
        /// Gets the angle with x and y.
        /// </summary>
        public static int GetAngle(int X, int Y)
        {
            if (X == 0 && Y == 0)
            {
                return 0;
            }
            if (X > 0 && Y >= 0)
            {
                if (Y < X)
                {
                    return Math.ATAN_TABLE[(Y << 7) / X];
                }
                return -Math.ATAN_TABLE[(X << 7) / Y] + 90;
            }
            else
            {
                int Num = Math.Abs(X);

                if (X <= 0 && Y > 0)
                {
                    if (Num < Y)
                    {
                        return Math.ATAN_TABLE[(Num << 7) / Y] + 90;
                    }
                    return -Math.ATAN_TABLE[(Y << 7) / Num] + 180;
                }
                else
                {
                    int Num2 = Math.Abs(Y);

                    if (X < 0 && Y <= 0)
                    {
                        if (Num2 < Num)
                        {
                            return Math.ATAN_TABLE[(Num2 << 7) / Num] + 180;
                        }

                        return -Math.ATAN_TABLE[(Num << 7) / Num2] + 270;
                    }
                    else
                    {
                        if (Num < Num2)
                        {
                            return Math.ATAN_TABLE[(Num << 7) / Num2] + 270;
                        }

                        return Math.NormalizeAngle360(-Math.ATAN_TABLE[(Num2 << 7) / Num] + 360);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the angle between the two angles.
        /// </summary>
        public static int GetAngleBetween(int Angle1, int Angle2)
        {
            return Math.Abs(Math.NormalizeAngle180(Angle1 - Angle2));
        }

        /// <summary>
        /// Gets the rotated x.
        /// </summary>
        public static int GetRotatedX(int X, int Y, int Angle)
        {
            X = X * Math.Cos(Angle) - Y * Math.Sin(Angle);
            return X >> Math.FIXED_SHIFT;
        }

        /// <summary>
        /// Gets the rotated y.
        /// </summary>
        public static int GetRotatedY(int X, int Y, int Angle)
        {
            Y = X * Math.Sin(Angle) + Y * Math.Cos(Angle);
            return Y >> Math.FIXED_SHIFT;
        }

        /// <summary>
        /// Normalizes valueA 180 angle.
        /// </summary>
        public static int NormalizeAngle180(int Angle)
        {
            Angle = Math.NormalizeAngle360(Angle);

            if (Angle >= 180)
            {
                return Angle - 360;
            }

            return Angle;
        }

        /// <summary>
        /// Normalizes valueA 360 angle.
        /// </summary>
        public static int NormalizeAngle360(int Angle)
        {
            Angle %= 360;

            if (Angle < 0)
            {
                return Angle + 360;
            }

            return Angle;
        }

        /// <summary>
        /// Returns the trigonometric sine of an angle.
        /// </summary>
        public static int Sin(int Angle)
        {
            Angle = Math.NormalizeAngle360(Angle);

            if (Angle < 180)
            {
                if (Angle > 90)
                {
                    Angle = 180 - Angle;
                }

                return Math.SIN_TABLE[Angle];
            }

            Angle -= 180;

            if (Angle > 90)
            {
                Angle = 180 - Angle;
            }

            return -Math.SIN_TABLE[Angle];
        }

        public static int Sqrt(int Value)
        {
            if (Value >= 0x10000)
            {
                int Num;

                if (Value >= 0x1000000)
                {
                    if (Value >= 0x10000000)
                    {
                        if (Value >= 0x40000000)
                        {
                            if (Value >= 0x7FFFFFFF)
                            {
                                return 0xFFFF;
                            }

                            Num = Math.SQRT_TABLE[Value >> 24] << 8;
                        }
                        else
                        {
                            Num = Math.SQRT_TABLE[Value >> 22] << 7;
                        }
                    }
                    else if (Value >= 0x4000000)
                    {
                        Num = Math.SQRT_TABLE[Value >> 20] << 6;
                    }
                    else
                    {
                        Num = Math.SQRT_TABLE[Value >> 18] << 5;
                    }

                    Num = Num + 1 + Value / Num >> 1;
                    Num = Num + 1 + Value / Num >> 1;

                    return Num * Num <= Value ? Num : Num - 1;
                }
                if (Value >= 0x100000)
                {
                    if (Value >= 0x400000)
                    {
                        Num = Math.SQRT_TABLE[Value >> 16] << 4;
                    }
                    else
                    {
                        Num = Math.SQRT_TABLE[Value >> 14] << 3;
                    }
                }
                else if (Value >= 0x40000)
                {
                    Num = Math.SQRT_TABLE[Value >> 12] << 2;
                }
                else
                {
                    Num = Math.SQRT_TABLE[Value >> 10] << 1;
                }

                Num = Num + 1 + Value / Num >> 1;

                return Num * Num <= Value ? Num : Num - 1;
            }
            else
            {
                if (Value >= 0x100)
                {
                    int Num;

                    if (Value >= 0x1000)
                    {
                        if (Value >= 0x4000)
                        {
                            Num = Math.SQRT_TABLE[Value >> 8] + 1;
                        }
                        else
                        {
                            Num = (Math.SQRT_TABLE[Value >> 6] >> 1) + 1;
                        }
                    }
                    else if (Value >= 0x400)
                    {
                        Num = (Math.SQRT_TABLE[Value >> 4] >> 2) + 1;
                    }
                    else
                    {
                        Num = (Math.SQRT_TABLE[Value >> 2] >> 3) + 1;
                    }

                    return Num * Num <= Value ? Num : Num - 1;
                }

                if (Value >= 0)
                {
                    return Math.SQRT_TABLE[Value] >> 4;
                }

                return -1;
            }
        }

        /// <summary>
        /// Clamps valueA value between valueA minimum int and maximum int value.
        /// </summary>
        public static int Clamp(int ClampValue, int MinValue, int MaxValue)
        {
            if (ClampValue >= MaxValue)
            {
                return MaxValue;
            }

            if (ClampValue <= MinValue)
            {
                return MinValue;
            }

            return ClampValue;
        }

        /// <summary>
        /// Returns the greater of two int values.
        /// </summary>
        public static int Max(int ValueA, int ValueB)
        {
            if (ValueA >= ValueB)
            {
                return ValueA;
            }

            return ValueB;
        }

        /// <summary>
        /// Returns the smaller of two double values.
        /// </summary>
        public static int Min(int ValueA, int ValueB)
        {
            if (ValueA <= ValueB)
            {
                return ValueA;
            }

            return ValueB;
        }
    }
}