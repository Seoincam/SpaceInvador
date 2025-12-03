using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TimeKit.Unity")]
[assembly: InternalsVisibleTo("TimeKit.Editor")]
#if DOTWEEN
[assembly: InternalsVisibleTo("TimeKit.DOTween")]
#endif