import matplotlib.pyplot as plt


class SplineTuple:
    def __init__(self, a, b, c, d, x):
        self.a = a
        self.b = b
        self.c = c
        self.d = d
        self.x = x


def CreateSplineTuple(X, Y, n):
    splines = []
    for index in range(0, n):
        splines.append(SplineTuple(Y[index], 0, 0, 0, X[index]))

    alpha = [0.0]
    beta = [0.0]

    # A = h
    # C = 2*(X[index+1] - X[index - 1])
    # B = X[index + 1] - X[index]
    # F = 6.0 * ((Y[index + 1] - Y[index]) / (X[index + 1] - X[index]) - (Y[index] - Y[index - 1]) / h)
    for index in range(1, n - 1):
        h = X[index] - X[index - 1]
        z = (h * alpha[index - 1] + 2 * (X[index + 1] - X[index - 1]))
        alpha.append(-(X[index + 1] - X[index]) / z)
        beta.append(
            (6.0 * ((Y[index + 1] - Y[index]) / (X[index + 1] - X[index]) - (Y[index] - Y[index - 1]) / h) -
             h * beta[index - 1]) / z
        )

    for index in range(n - 2, 0, -1):
        splines[index].c = alpha[index] * splines[index + 1].c + beta[index]

    for index in range(n - 1, 0, -1):
        h = X[index] - X[index - 1]
        splines[index].d = (splines[index].c - splines[index - 1].c) / h
        splines[index].b = h * (2.0 * splines[index].c + splines[index - 1].c) / 6.0 + (Y[index] - Y[index - 1]) / h

    return splines


def Interpolate(splines, x):
    n = len(splines)
    j = n - 1
    if x <= splines[0].x:
        j = 0
    elif x >= splines[n - 1].x:
        j = n - 1
    else:
        index = 0
        while index + 1 < j:
            k = index + (j - index) // 2
            if x <= splines[k].x:
                j = k
            else:
                index = k

    dx = x - splines[j].x
    return splines[j].a + (
            splines[j].b + (splines[j].c * 0.5 + splines[j].d * dx * 0.16666666666667) * dx) * dx


x = [0, 1, 2, 3, 4]
y = [1, 2, 17, 82, 257]

spline = CreateSplineTuple(x, y, len(x))

plt.scatter(x, y)
plt.plot(x, y)

for i in [0, 2, 4, 6]:
    new_x = (x[i] + x[i + 1]) / 2
    plt.scatter(new_x, Interpolate(spline, new_x))
    x.insert(i + 1, new_x)
    new_y = Interpolate(spline, new_x)
    y.insert(i + 1, new_y)

plt.show()

print("X = ", x, "\nY = ", y)
