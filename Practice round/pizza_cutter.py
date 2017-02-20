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

print(metadata)
print(pizza)

num_slices = math.ceil(pizza.get_cell_count() / metadata['max_cells_per_slice'])

slices = list()

for i in range(0, metadata['rows'] - 1, metadata['max_cells_per_slice'] // metadata['rows']):
        for j in range(0, (metadata['columns'] // 2), metadata['max_cells_per_slice'] // metadata['rows']):
                slices.append(pizza.cut_cells((i, j), (i + (metadata['max_cells_per_slice'] // metadata['rows']), j + 1)))

        for j in range(metadata['columns'] - 1, (metadata['columns'] // 2), -(metadata['max_cells_per_slice'] // metadata['rows'])):
                slices.append(pizza.cut_cells((i, j - 1), (i + (metadata['max_cells_per_slice'] // metadata['rows']), j)))

        slices.append(pizza.cut_cells((i, (metadata['columns'] // 2)), (i + (metadata['max_cells_per_slice'] // metadata['rows']), (metadata['columns'] // 2))))

print(pizza)
print(slices)

with open('outputs\\' + file_name + '.out', 'w') as f:
        f.write(str(len(slices)) + '\n')
        for s in slices:
                f.write(s[0] + '\n')

