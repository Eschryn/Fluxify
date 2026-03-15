// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Buffers;
using System.Runtime.CompilerServices;

namespace Fluxify.Commands.TextCommand;

public sealed class CommandTokenizer(ReadOnlyMemory<char> input, int offset = 0)
{
    private const string WhiteSpaceChars =
        "\u0009\u000A\u000B\u000C\u000D\u0020\u0085\u00A0\u1680"
        + "\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008"
        + "\u2009\u200A\u2028\u2029\u202F\u205F\u3000";

    private const string WhiteSpaceCharsNoBreak =
        "\u180E\u200B\u200C\u200D\u2060\uFEFF";

    private const string AllWhiteSpaceChars = WhiteSpaceChars + WhiteSpaceCharsNoBreak;
    private const string DelimiterChars = "#:<>@&!";

    private (ReadOnlyMemory<char> value, Range range)? _cached;
    private int _offset = offset;
    private ReadOnlyMemory<char> _input = input;

    private static readonly SearchValues<char> Space = SearchValues.Create(AllWhiteSpaceChars);

    private static readonly SearchValues<char> DelimitersWithSpace =
        SearchValues.Create(AllWhiteSpaceChars + DelimiterChars);

    private static readonly SearchValues<char> Delimiters = SearchValues.Create(DelimiterChars);

    private ReadOnlyMemory<char> NextChar()
    {
        try
        {
            return _input[..1];
        }
        finally
        {
            SetNewPosition(1);
        }
    }

    private void SetNewPosition(int relativeOffset)
    {
        if (relativeOffset >= _input.Length)
        {
            _offset += _input.Length;
            _input = ReadOnlyMemory<char>.Empty;
            
            return;
        }
        
        var newRelativeOffset = relativeOffset;
        
        // trim space
        if (AllWhiteSpaceChars.Contains(_input.Span[relativeOffset]))
        {
            newRelativeOffset = relativeOffset + _input.Span[relativeOffset..].IndexOfAnyExcept(Space);
        }

        _offset += newRelativeOffset;
        _input = _input[newRelativeOffset..];
    }

    public ReadOnlyMemory<char> Peek(out Range range)
    {
        _cached = (Next(DelimitersWithSpace, out range), range);

        return _cached.Value.value;
    }

    public void ConsumeNext() => _ = Next(out _);

    public ReadOnlyMemory<char> Next(out Range range)
    {
        if (_cached is not null)
        {
            range = _cached.Value.range;
            var val = _cached.Value.value;
            _cached = null;
            return val;
        }

        return Next(DelimitersWithSpace, out range);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyMemory<char> NextNoSpace(out Range range)
    {
        return Next(Delimiters, out range);
    }

    public ReadOnlyMemory<char> Next(SearchValues<char> delimiters, out Range range)
    {
        var nextIndex = _input.Span.IndexOfAny(delimiters);
        if (nextIndex == 0)
            nextIndex = 1;
        try
        {
            if (nextIndex == -1)
            {
                range = _offset.._input.Length;
                nextIndex = _input.Length;

                return _input;
            }

            range = _offset..nextIndex;
            return _input[..nextIndex];
        }
        finally
        {
            SetNewPosition(nextIndex);
        }
    }

    public static CommandTokenizer WithoutPrefix(string prefix, string messageContent)
        => new(messageContent.AsMemory(prefix.Length), prefix.Length);
}
