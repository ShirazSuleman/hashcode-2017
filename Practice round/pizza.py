class Pizza(object):
    def __init__(self, grid):
        self.grid = grid

    def __str__(self):
        result = ''
        for row in self.grid:
            result += (' ').join(row) + '\n'
        return result

    def get_cell_value(self, cell):
        return self.grid[cell[0]][cell[1]]

    def get_cell_count(self):
        num_cells = 0
        for row in self.grid:
            for col in row:
                num_cells += 1
        return num_cells

    def get_cell_value_counts(self):
        ingredient_count = {}
        for row in self.grid:
            for col in row:
                ingredient_count[col] = ingredient_count.get(col, 0) + 1
        return ingredient_count

    def cut_cells(self, cell1, cell2):
        block = list()
        for r in range(cell1[0], cell2[0] + 1):
            for c in range(cell1[1], cell2[1] + 1):
                block.append(self.grid[r][c])
                self.grid[r][c] = 'X'
        return (str(cell1[0]) + ' ' + str(cell1[1]) + ' ' + str(cell2[0]) + ' ' + str(cell2[1]) , block,)
