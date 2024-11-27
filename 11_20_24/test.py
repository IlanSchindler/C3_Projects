import networkx as nx
from matplotlib import pyplot as plt
import numpy as np

def on_click(event):
    if event.inaxes is not None:
        x, y = event.xdata, event.ydata
        for node, (node_x, node_y) in pos.items():
            if abs(x - node_x) < 0.1 and abs(y - node_y) < 0.1:
                # Change color of the clicked node
                node_colors[node] = 'red'
                plt.clf()
                nx.draw(G, pos, node_color=node_colors, with_labels=True)
                plt.draw()
                break

moves = [(-1,-2),(1,-2),(-2,-1),(2,-1),(-2,1),(2,1),(-1,2),(1,2)]
board = [[8*x+y for y in range(8)] for x in range(8)]
G = nx.Graph()
for r in board:
    for c in r:
        G.add_node(c)
pos = {}
for i in range(len(board)):
    for j in range(len(board[0])):
        #pos = pos + {board[i][j]: [i,j]}
        for m in moves:
            if i + m[0] >= 0 and i + m[0] <8 and j+m[1]>=0 and j+m[1]<8:
                G.add_edge(board[i][j], board[i+m[0]][j+m[1]])

# Calculate degree for each node
degree_dict = dict(G.degree())

# Separate nodes by degree
nodes_by_degree = {}
for node, degree in degree_dict.items():
    if degree not in nodes_by_degree:
        nodes_by_degree[degree] = []
    nodes_by_degree[degree].append(node)

shells = [[0,7,56,63]]
print(list(dict(G.adj[0]).keys()))
i = 0
while True:
    newNodes = []
    for n in shells[i]:
        newNodes += list(dict(G.adj[n]).keys())
    oldNodes = [x for shell in shells for x in shell]
    newNodes = [n for n in newNodes if n not in oldNodes]
    if(newNodes == []):
        break
    shells += [list(np.unique(newNodes))]
    i+=1

nodeorder = [0]
for i in range(len(G.nodes)):
    print(i)
    opts = list(dict(G.adj[nodeorder[i]]).keys())
    opts.sort()
    opts = [n for n in opts if n not in nodeorder]
    middle = len(opts)//2
    print(opts)
    nodeorder += [opts[middle]]
    print(nodeorder)

    print()

print(shells)

pos = {board[i][j]: (i,j) for i in range(len(board)) for j in range(len(board[0]))}
#pos = nx.spring_layout(G)
#pos = nx.spiral_layout(G)
#pos = nx.shell_layout(G, nlist=shells)
#pos = nx.planar_layout(G)
#pos = nx.spectral_layout(G, scale = 5)
#pos = nx.random_layout(G)
#pos = nx.circular_layout(G)
#pos = nx.multipartite_layout(G)
#pos = nx.rescale_layout(G)
#pos = nx.bfs_layout(G,0)
#pos = nx.arf_layout(G)
#pos = nx.bipartite_layout(G)
#pos = nx.kamada_kawai_layout(G)
#pos = nx.spring_layout(G,pos = pos, fixed=[0,7,56,63],iterations=500,k=.1)
#pos = nx.spring_layout(G,pos = pos, iterations=500,k=.1)
#pos = nx.rescale_layout(np.array(list(pos.values())), 1)
#pos=nx.spring_layout(G,k=.01,iterations=500)
#pos=nx.bfs_layout(G,0)
#print(pos)
#nx.draw_networkx(G, pos)


node_colors = ['blue'] * len(G.nodes())
fig, ax = plt.subplots()
nx.draw(G, pos, node_color=node_colors, with_labels=True)

fig.canvas.mpl_connect('button_press_event', on_click)

plt.show()

