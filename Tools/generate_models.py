"""Generate original, meter-scale Wavefront models; no external packages required."""
from pathlib import Path
ROOT = Path(__file__).resolve().parents[1] / 'Assets' / 'Models'
def export(name, boxes):
    lines = ['# Aula Inclusiva 3D - original geometry, units: meters', 'o ' + name]
    count = 0
    for cx,cy,cz,sx,sy,sz in boxes:
        for x,y,z in [(-1,-1,-1),(1,-1,-1),(1,1,-1),(-1,1,-1),(-1,-1,1),(1,-1,1),(1,1,1),(-1,1,1)]:
            lines.append(f'v {cx+x*sx/2:g} {cy+y*sy/2:g} {cz+z*sz/2:g}')
        for face in [(1,4,3,2),(5,6,7,8),(1,5,8,4),(2,3,7,6),(4,8,7,3),(1,2,6,5)]:
            lines.append('f ' + ' '.join(str(count+i) for i in face))
        count += 8
    (ROOT / (name + '.obj')).write_text('\n'.join(lines)+'\n', encoding='utf-8')
export('Mesa', [(0,.78,0,1.4,.12,.8)] + [(x,.36,z,.09,.72,.09) for x in [-.58,.58] for z in [-.28,.28]])
export('Silla', [(0,.45,0,.52,.10,.52),(0,.80,-.22,.52,.65,.08)] + [(x,.2,z,.06,.4,.06) for x in [-.20,.20] for z in [-.20,.20]])
