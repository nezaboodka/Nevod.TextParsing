from urllib import request
from os import path, system, sys
import re
from collections import defaultdict


UNICODE_VERSION = '6.3.0' 
BASE_URL = r'http://www.unicode.org/Public/{}/ucd/auxiliary/'.format(UNICODE_VERSION)
WORD_BREAK_PROPERTY_FILENAME = 'WordBreakProperty.txt'

HEXADECIMAL = r'[0-9A-F]+'
SINGLE_PROPERTY = r'^ *({HEXADECIMAL}) *; *(\w+)'.format(HEXADECIMAL=HEXADECIMAL)
RANGE_PROPERTY = r'^ *({HEXADECIMAL})\.\.({HEXADECIMAL}) *; *(\w+)'.format(HEXADECIMAL=HEXADECIMAL)

WORD_BREAK_PROPERTY_TEMPLATE = "new WordBreakProperty('{}', '{}', WordBreakPropertyType.{})"

TAB_LENGTH = 4
INDENT = TAB_LENGTH * 3

SOURCE_PATH = 'WordBreakPropertyTable.cs'
TEMPLATE_PATH = 'WordBreakPropertyTable.cs.template'
PROPERTIES_PLACEHOLDER = r'/* %properties% */'


def download_file(filename):
    return request.urlopen(BASE_URL + filename).read().decode('utf-8')


def fetch_file(filename):
    try:
        if not path.exists(filename):
            with open(WORD_BREAK_PROPERTY_FILENAME) as output:
                output.write(download_file(filename))
    except IOError:
        print("Can't load {}.".format(filename), file=sys.stderr)
        sys.exit(1)


def merge_ranges(properties):
    result = defaultdict(list)
    for property, ranges in properties.items():
        current_result = result[property]
        current_result.append(ranges[0])
        
        for range in ranges[1:]:
            if range[0] == current_result[-1][1] + 1:
                current_result[-1] = (current_result[-1][0], range[1])
            else:
                current_result.append(range)

    return result


def load_properties(filename):
    fetch_file(filename)

    properties = defaultdict(list)

    single_value_property_re = re.compile(SINGLE_PROPERTY)
    range_value_property_re = re.compile(RANGE_PROPERTY)

    two_bytes = lambda x: x <= 0xFFFF

    for line in open(filename):
        property = None
        low_bound = None
        high_bound = None

        single_property_match = single_value_property_re.match(line)
        if single_property_match:
            low_bound = int(single_property_match.group(1), 16)
            high_bound = low_bound
            property = single_property_match.group(2)
        else:
            range_property_match = range_value_property_re.match(line)
            if range_property_match:
                low_bound = int(range_property_match.group(1), 16)
                high_bound = int(range_property_match.group(2), 16)
                property = range_property_match.group(3)
            else:
                continue
        if two_bytes(low_bound) and two_bytes(high_bound):
            properties[property].append((low_bound, high_bound))
    
    return merge_ranges(properties)


def format_char(c):
    return r"\u{:x}".format(c)


def format_property(property):
    return WORD_BREAK_PROPERTY_TEMPLATE.format(format_char(property[0]), format_char(property[1]), property[2])


def format_table(properties_sequence, indent=INDENT):
    result = format_property(properties_sequence[0]) + ',\n'
    
    first = True
    for property in properties_sequence:
        property_string = format_property(property) + ','
        if first:
            result += ' ' * INDENT + property_string + ' '
        else:
            result += property_string + '\n'

        first = not first

    return result


def generate_table(properties):
    properties_sequence = []
    properties_sequence.sort(key=lambda x: x[0])
    first = True
    for property, ranges in properties.items():
        for low_bound, high_bound in ranges:
            properties_sequence.append((low_bound, high_bound, property))
    properties_sequence.sort(key=lambda x: x[0])
    return format_table(properties_sequence)

def produce_source(table):
    with open(TEMPLATE_PATH) as input:
        content = input.read()

    content = content.replace(PROPERTIES_PLACEHOLDER, table)

    with open(SOURCE_PATH, 'w') as output:
        output.write(content)



def main():
    #ranges = {'a': [(1, 1), (2, 2), (3, 3), (5, 19), (20, 20)]}
    #print(merge_ranges(ranges))
    #with open('out', 'w') as output:
    #    print(generate_table(load_properties(WORD_BREAK_PROPERTY_FILENAME)), file=output)
    properties = load_properties(WORD_BREAK_PROPERTY_FILENAME)
    table = generate_table(properties)
    produce_source(table)

if __name__ == '__main__':
    main()