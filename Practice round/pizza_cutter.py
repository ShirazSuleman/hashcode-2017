import pizza
import math

file_name = 'example'

with open('inputs\\' + file_name + '.in', 'r') as f:
        lines = f.readlines()

metadata = dict()
metadata_inputs = lines[0][:-1].split(' ')

metadata['rows'] = int(metadata_inputs[0])
metadata['columns'] = int(metadata_inputs[1])
metadata['min_ingredient_per_slice'] = int(metadata_inputs[2])
metadata['max_cells_per_slice'] = int(metadata_inputs[3])

pizza_list = list()

for i in range(1, len(lines)):
	pizza_row = list()
	for j in range(len(lines[i]) - 1):
		pizza_row.append(lines[i][j])
	pizza_list.append(pizza_row)

pizza = pizza.Pizza(pizza_list)

#print(metadata)
#print(pizza)

num_slices = math.ceil(pizza.get_cell_count() / metadata['max_cells_per_slice'])

slices = list()

# min_cells = metadata['min_ingredient_per_slice'] * 2
min_cells = metadata['max_cells_per_slice'] if metadata['rows'] > metadata['max_cells_per_slice'] else metadata['rows']

column_slices = {}
potential_column_scores = {}

column = 0

while column < metadata['columns']:
        potential_column_scores[column] = 0
        column_slices[column] = []
        row = 0
        while row < metadata['rows']:
                ingredient_count = {}
                step = (row + min_cells) if (row + min_cells) < metadata['rows'] else metadata['rows']
                for cell in range(row, step):
                        ingredient_count[pizza.get_cell_value((cell, column))] = ingredient_count.get(pizza.get_cell_value((cell, column)), 0) + 1
                
                if ingredient_count.get('T', 0) >= metadata['min_ingredient_per_slice'] and ingredient_count.get('M', 0) >= metadata['min_ingredient_per_slice']:
                        potential_column_scores[column] = potential_column_scores.get(column, 0) + (ingredient_count.get('T', 0) + ingredient_count.get('M', 0))
                        column_slices[column].append(((row, column, (row + step - 1), column)))
                        row += step
                else:
                        row += 1
        column += 1

# min_cells = metadata['min_ingredient_per_slice'] * 2
min_cells = metadata['max_cells_per_slice'] if metadata['columns'] > metadata['max_cells_per_slice'] else metadata['columns']

row_slices = {}
potential_row_scores = {}

row = 0

while row < metadata['rows']:
        potential_row_scores[row] = 0
        row_slices[row] = []
        column = 0
        while column < metadata['columns']:
                ingredient_count = {}
                step = (column + min_cells) if (column + min_cells) < metadata['columns'] else metadata['columns']
                for cell in range(column, step):
                        ingredient_count[pizza.get_cell_value((row, cell))] = ingredient_count.get(pizza.get_cell_value((row, cell)), 0) + 1
                
                if ingredient_count.get('T', 0) >= metadata['min_ingredient_per_slice'] and ingredient_count.get('M', 0) >= metadata['min_ingredient_per_slice']:
                        potential_row_scores[row] = potential_row_scores.get(row, 0) + (ingredient_count.get('T', 0) + ingredient_count.get('M', 0))
                        row_slices[row].append(((row, column, row, (column + step - 1))))
                        column += step
                else:
                        column += 1
        row += 1

print('Potential Column Scores: ')
print(potential_column_scores)

print('Column Slices: ')
print(column_slices)

print('Total Column Score: ')
print(sum(potential_column_scores.values()))

print('Potential Row Scores: ')
print(potential_row_scores)

print('Row Slices: ')
print(row_slices)

print('Total Row Score: ')
print(sum(potential_row_scores.values()))

#print(pizza)
#print(slices)

with open('outputs\\' + file_name + '.out', 'w') as f:
        f.write(str(len(slices)) + '\n')
        for s in slices:
                f.write(s[0] + '\n')

