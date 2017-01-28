from tables_generator import *
from output_formatter import *

def main():
    property_values, property_types = get_properties(WORD_BREAK_PROPERTY_FILENAME)
    expanded_table = expand_property_values(property_values)
    compressed_table = compress_table(expanded_table)    
    properties_table = generate_table(compressed_table)
    types_table = generate_types_table(property_types)
    short_names_declaration = generate_short_names_declaration(SHORT_NAMES)
    write_result(properties_table, types_table, short_names_declaration, get_output_filename())

if __name__ == '__main__':
    main()