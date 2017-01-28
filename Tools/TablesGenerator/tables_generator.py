#!/usr/bin/env python3

# This script uses WordBreakProperty.txt from 
# http://www.unicode.org/Public/6.3.0/ucd/auxiliary/WordBreakProperty.txt to
# produce C# source.
# Unicode version can be set by UNICODE_VERSION constant. 

from urllib import request
from os import path, system, sys
import re
from collections import defaultdict
import argparse
from output_formatter import *

UNICODE_VERSION = '6.3.0' 
BASE_URL = r'http://www.unicode.org/Public/{}/ucd/auxiliary/'.format(UNICODE_VERSION)
WORD_BREAK_PROPERTY_FILENAME = 'WordBreakProperty.txt'

NAMES_REPLACEMENTS = {
    'ALetter': 'AlphabeticLetter', 
    'CR': 'CarriageReturn', 
    'Extend': 'Extender', 
    'ExtendNumLet': 'ExtenderForNumbersAndLetters', 
    'LF': 'LineFeed', 
    'MidNum': 'MidNumber', 
    'MidNumLet': 'MidNumberAndLetter'
}


HEXADECIMAL = r'[0-9A-F]+'
SINGLE_PROPERTY = r'^ *({HEXADECIMAL}) *; *(\w+)'.format(HEXADECIMAL=HEXADECIMAL)
RANGE_PROPERTY = r'^ *({HEXADECIMAL})\.\.({HEXADECIMAL}) *; *(\w+)'.format(HEXADECIMAL=HEXADECIMAL)


def get_properties(filename):
    property_values, property_types = load_properties(filename)
    add_custom_values(property_values, property_types )
    return merge_ranges(property_values), sorted(property_types)


def expand_property_values(property_values):
    result = ['Any']*65536
    for property, ranges in property_values.items():
        for bound in ranges:
            for i in range(bound[0], bound[1] + 1):
                result[i] = property
    return result


def get_gaps_for(expanded_table, property_name):
    result = []
    for high in range(256):
        for low in range(256):
            current_index = (high << 8) + low            
            current_property = expanded_table[current_index]
            if current_property != property_name:
                break
        else:            
            result.append(high)
        
    return result


def compress_table(expanded_table):
    compressing_values = {'Any':'AnyArray', 'AlphabeticLetter':'AlphabeticLetterArray'}

    result = [None] * 256
    gaps_groups = {}
    for compressing_property in compressing_values:
        gaps_groups[compressing_values[compressing_property]] = get_gaps_for(expanded_table, compressing_property)
      
    for high in range(256):
        current_array_name = ''
        for array_name, gaps in gaps_groups.items():
            if high in gaps:
                current_array_name = array_name
        if not current_array_name:
            sub_array = [None]*256
            for low in range(256):
                index = (high << 8) + low
                sub_array[low] = expanded_table[index]
            result[high] = sub_array
        else:
            result[high] = current_array_name
    return result
    

def get_output_filename():
    argument_parser = argparse.ArgumentParser(description='Generate C# code from WordBreakProperty.txt.')
    argument_parser.add_argument('output_filename', type=str, help='path to the output file')
    args = argument_parser.parse_args()
    return args.output_filename

    
def load_properties(filename):
    fetch_file(filename)

    property_values = defaultdict(list)
    property_types = {'Any', 'Empty'}

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
            property_values[property_type].append((low_bound, high_bound))
            property_types.add(property_type)
    
    return property_values, property_types

def add_custom_values(property_values, property_types):
    WHITESPACE_PROPERTY = "Whitespace"
    SPACE_CODE = ord(' ')
    TAB_CODE = ord('\t')    

    property_types.add(WHITESPACE_PROPERTY)
    property_values[WHITESPACE_PROPERTY].extend([
        (SPACE_CODE, SPACE_CODE),
        (TAB_CODE, TAB_CODE)
    ])


def format_property_type(property_type):
    property_type = property_type.replace('_', '')
    return NAMES_REPLACEMENTS.get(property_type, property_type)


def fetch_file(filename):
    try:
        if not path.exists(filename):
            with open(WORD_BREAK_PROPERTY_FILENAME, 'w') as output:
                output.write(download_file(filename))
    except IOError:
        print("Can't load {}.".format(filename), file=sys.stderr)
        sys.exit(1)


def download_file(filename):
    return request.urlopen(BASE_URL + filename).read().decode('utf-8')


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



