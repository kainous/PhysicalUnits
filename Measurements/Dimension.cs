using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Numerics;

namespace System.Measurements {
    public sealed partial class Dimension {
        private readonly IDictionary<char, Dimension> _bySymbol =
            new Dictionary<char, Dimension>();

        private readonly IDictionary<string, Dimension> _byTextID =
            new Dictionary<string, Dimension>(StringComparer.OrdinalIgnoreCase);

        public string TextID { get; }
        public char Symbol { get; }

        internal Dimension(string textID, char symbol) {
            symbol = char.ToUpper(symbol);
            if (!char.IsLetterOrDigit(symbol)) {
                throw new ArgumentException("Symbol must be a character", nameof(symbol));
            }

            if (string.IsNullOrWhiteSpace(textID)) {
                throw new ArgumentException("Text ID must contain data", nameof(TextID));
            }

            //var sdfoin = SquareMatrix2.2

            TextID = textID;
            Symbol = symbol;

            _bySymbol.Add(symbol, this);
            _byTextID.Add(textID, this);
        }
    }
}
