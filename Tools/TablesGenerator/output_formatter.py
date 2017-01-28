TAB_LENGTH = 4

SHORT_NAMES ={
    'AlphabeticLetter':'AL',
    'Any': 'A',
    'CarriageReturn': 'CR',
    'DoubleQuote': 'DQ',    
    'Extender': 'E',
    'ExtenderForNumbersAndLetters': 'ENL',
    'Format': 'F',
    'HebrewLetter': 'HL',
    'Katakana': 'K',
    'LineFeed': 'LF',
    'MidLetter': 'ML',
    'MidNumber': 'MN',
    'MidNumberAndLetter': 'MNL',
    'Newline': 'NL',
    'Numeric': 'NM',
    'SingleQuote': 'SQ',
    'Whitespace': 'W',
}

SHORT_NAME_DECLARATION_TEMPLATE = 'private const WordBreak {} = WordBreak.{}'

def generate_table(compressed_table):
    return format_table(compressed_table, format_func=format_low_array, count_in_line=10, indent=TAB_LENGTH * 2)


def generate_types_table(property_types):
    return format_table(list(property_types), count_in_line=1, indent=TAB_LENGTH * 2)


def generate_short_names_declaration(short_names):
    result = format_table(short_names, count_in_line=1, format_func=format_short_name_declaration, indent=TAB_LENGTH * 2, separator=';')
    result = result[:len(result) - 2] + ';'
    return result


def format_short_name_declaration(name):
    return SHORT_NAME_DECLARATION_TEMPLATE.format(SHORT_NAMES[name], name), False


def write_result(array_elements, word_break_values, short_name_declarations, output_filename):
    with open(output_filename, 'w') as output:
        output.write('Enum values:\n{}'.format(word_break_values))
        output.write('\n\n')
        output.write('Short names declaration:\n{}'.format(short_name_declarations))
        output.write('\n\n')
        output.write('Array elements:\n{}'.format(array_elements))


def format_low_array(low_array):    
    if isinstance(low_array, list):
        result = 'new []{'
        result += format_table(low_array, format_func=format_enum_value, count_in_line=30, indent=TAB_LENGTH * 3, init_position=1)
        result += '}'
        result = result, True
    else:
        result = low_array, False
    return result


def format_enum_value(enum_value):
    if enum_value in SHORT_NAMES:
        short_name = SHORT_NAMES[enum_value]
    else:
        short_name = enum_value
    return short_name, False


def format_table(items, count_in_line, indent, format_func=lambda x:(x, False), init_position=0, separator=','):
    result = ''
    elements_in_line = init_position
    for index, item in enumerate(items):
        elements_in_line += 1

        item_string, new_line = format_func(item)
        if index != len(items) - 1:
            item_string += separator
        if new_line and result and result[len(result) - 1] != '\n':
            result += '\n'
            elements_in_line = 1

        if elements_in_line == 1:
            item_string = ' ' * indent + item_string + ' '
        if elements_in_line == count_in_line or new_line:
            item_string += '\n'
        if item_string[len(item_string) - 1] == '\n':
            elements_in_line = 0

        result += item_string
    
    return result