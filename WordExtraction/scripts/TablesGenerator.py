#!/usr/bin/env python3

# This script uses WordBreakProperty.txt from 
# http://www.unicode.org/Public/6.3.0/ucd/auxiliary/WordBreakProperty.txt to
# produce C# source from WordBreakPropertyTable.cs.template.
# Unicode version can be set by UNICODE_VERSION constant. Keep in mind, that
# you should modify C# code in WordExtractor.cs according to the new version 
# in case of changing this constant.
#
# NOTE: This should not require frequent updates, so you should use pregenerated source from
# the parent folder.

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

WORD_BREAK_PROPERTY_TEMPLATE = "new SymbolTypeInfo('{}', '{}', SymbolType.{})"
ENUM_VALUE_TEMPLATE = 'ST_{}'

TAB_LENGTH = 4

CLASS_NAME = 'SymbolTable'
SOURCE_PATH = CLASS_NAME + '.cs'
TEMPLATE_PATH = CLASS_NAME + '.template.cs'
PROPERTIES_PLACEHOLDER = r'/* %types_info% */'
TYPES_PLACEHOLDER = r'/* %types% */'


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
    property_types = {format_property_type('any')}

    single_value_property_re = re.compile(SINGLE_PROPERTY)
    range_value_property_re = re.compile(RANGE_PROPERTY)

    two_bytes = lambda x: x <= 0xFFFF

    for line in open(filename):
        property_type = None
        low_bound = None
        high_bound = None

        single_property_match = single_value_property_re.match(line)
        if single_property_match:
            low_bound = int(single_property_match.group(1), 16)
            high_bound = low_bound
            property_type = single_property_match.group(2)
        else:
            range_property_match = range_value_property_re.match(line)
            if range_property_match:
                low_bound = int(range_property_match.group(1), 16)
                high_bound = int(range_property_match.group(2), 16)
                property_type = range_property_match.group(3)
            else:
                continue
        if two_bytes(low_bound) and two_bytes(high_bound):
            property_type = format_property_type(property_type)
            properties[property_type].append((low_bound, high_bound))
            property_types.add(property_type)
    
    return merge_ranges(properties), sorted(property_types)


def format_char(c):
    return r"\x{:x}".format(c)


def format_property(property):
    return WORD_BREAK_PROPERTY_TEMPLATE.format(format_char(property[0]), format_char(property[1]), property[2])


def format_property_type(property_type):
    return ENUM_VALUE_TEMPLATE.format(property_type.upper())


def format_table(items, count_in_line, indent, format_func=lambda x:x, init_position=0):
    result = ''
    i = init_position
    for item in items:
        i += 1

        item_string = format_func(item) + ','

        if i == 1:
            item_string = ' ' * indent + item_string + ' '
        if i == count_in_line:
            item_string += '\n'
            i = 0

        result += item_string
    
    return result


def generate_table(properties):
    properties_sequence = []
    properties_sequence.sort(key=lambda x: x[0])
    
    for property, ranges in properties.items():
        for low_bound, high_bound in ranges:
            properties_sequence.append((low_bound, high_bound, property))
    properties_sequence.sort(key=lambda x: x[0])
    return format_table(properties_sequence, format_func=format_property, count_in_line=2, indent=TAB_LENGTH * 3, init_position=1)


def generate_types_table(property_types):
    return format_table(list(property_types), count_in_line=1, indent=TAB_LENGTH * 2)


def produce_source(table, enum_values):
    with open(TEMPLATE_PATH) as input:
        content = ''.join(input.readlines())

    content = content.replace(PROPERTIES_PLACEHOLDER, table).replace(TYPES_PLACEHOLDER, enum_values)

    with open(SOURCE_PATH, 'w') as output:
        output.write(content)


def main():
    property_values, property_types = load_properties(WORD_BREAK_PROPERTY_FILENAME)
    properties_table = generate_table(property_values)
    types_table = generate_types_table(property_types)
    produce_source(properties_table, types_table)


if __name__ == '__main__':
    main()
