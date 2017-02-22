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

potential_scores = {}
min_cells = metadata['min_ingredient_per_slice'] * 2


column = 0

while column < metadata['columns']:
        potential_scores[column] = 0
        row = 0
        while row < metadata['rows']:
                ingredient_count = {}
                if (row + min_cells) > metadata['rows']:
                        break
                for cell in range(row, row + min_cells):
                        ingredient_count[pizza.get_cell_value((cell, column))] = ingredient_count.get(pizza.get_cell_value((cell, column)), 0) + 1
                
                if ingredient_count.get('T', 0) >= metadata['min_ingredient_per_slice'] and ingredient_count.get('M', 0) >= metadata['min_ingredient_per_slice']:
                        potential_scores[column] = potential_scores.get(column, 0) + 1
                        row += min_cells
                else:
                        row += 1
        column += 1

print('Potential Scores: ')
print(potential_scores)
#print(pizza)
#print(slices)

with open('outputs\\' + file_name + '.out', 'w') as f:
        f.write(str(len(slices)) + '\n')
        for s in slices:
                f.write(s[0] + '\n')

