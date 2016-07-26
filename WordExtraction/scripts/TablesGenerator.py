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
        
        properties[property].append((low_bound, high_bound))
    
    return properties


def main():
    ranges = {'a': [(1, 1), (2, 2), (3, 3), (5, 19), (20, 20)]}
    print(merge_ranges(ranges))

if __name__ == '__main__':
    main()