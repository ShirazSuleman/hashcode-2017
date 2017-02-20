class Pizza(object):
    def __init__(self, pizza):
        self.pizza = pizza

    def __str__(self):
        result = ''
        for row in self.pizza:
            result += (' ').join(row) + '\n'
        return result

    def get_cell_ingredient(self, cell):
        return self.pizza[cell[0]][cell[1]]

    def cut_slice(self, cell1, cell2):
        slice = list()
        for r in range(cell1[0], cell2[0] + 1):
            for c in range(cell1[1], cell2[1] + 1):
                slice.append(self.pizza[r][c])
                self.pizza[r][c] = 'X'
        return slice
