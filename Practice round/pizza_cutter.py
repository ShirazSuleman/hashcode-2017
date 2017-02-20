import pizza

file_name = 'example.in'

file = open(file_name)
lines = file.readlines()

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

file.close()

pizza = pizza.Pizza(pizza_list)

print(metadata)
print(pizza)

slices = list()
slices.append(pizza.cut_slice((0, 0), (2, 1)))
slices.append(pizza.cut_slice((0, 2), (2, 2)))
slices.append(pizza.cut_slice((0, 3), (2, 4)))

print(pizza)
print(slices)
