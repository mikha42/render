using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace render
{
    static class vector2pixel
    {
        public static Tuple<bool, int, int, double> draw3D(int width, int height, vector3 point, vector3 cam, vector3 camdir_, vector3 camup, vector3 camleft, double clip, double zoom = 1)
        {
            vector3 camdir = camdir_ * clip;
            Tuple<bool, int, int, double> offscreen = new Tuple<bool, int, int, double>(false, -1, -1, 0);
            vector3 PC = cam - point;
            double aspect = (double)width / height;


            vector3 val = point - cam - camdir;
            double det = camleft.x * camup.y * cam.z - camleft.x * camup.y * point.z - camleft.x * camup.z * cam.y
                       + camleft.x * camup.z * point.y - camleft.y * camup.x * cam.z + camleft.y * camup.x * point.z
                       + camleft.y * camup.z * cam.x - camleft.y * camup.z * point.x + camleft.z * camup.x * cam.y
                       - camleft.z * camup.x * point.y - camleft.z * camup.y * cam.x + camleft.z * camup.y * point.x;

            if (det == 0)
                return offscreen;

            vector3 mat1 = new vector3(
                camup.y * cam.z - camup.y * point.z - camup.z * cam.y + camup.z * point.y,
                -camup.x * cam.z + camup.x * point.z + camup.z * cam.x - camup.z * point.x,
                camup.x * cam.y - camup.x * point.y - camup.y * cam.x + camup.y * point.x
                );
            vector3 mat2 = new vector3(
                -camleft.y * cam.z + camleft.y * point.z + camleft.z * cam.y - camleft.z * point.y,
                camleft.x * cam.z - camleft.x * point.z - camleft.z * cam.x + camleft.z * point.x,
                -camleft.x * cam.y + camleft.x * point.y + camleft.y * cam.x - camleft.y * point.x
                );
            vector3 mat3 = new vector3(
                camleft.y * camup.z - camleft.z * camup.y,
                -camleft.x * camup.z + camleft.z * camup.x,
                camleft.x * camup.y - camleft.y * camup.x
                );

            vector3 ans = new vector3(
                vector3.dot(val, mat1 / det),
                vector3.dot(val, mat2 / det),
                vector3.dot(val, mat3 / det)
                );

            bool valid = true;
            if (ans.z * ans.z > 1)
                valid = false;

            clip *= zoom;

            int px = (int)Math.Round(((ans.x + (clip * aspect)) / (2 * clip * aspect)) * width);
            int py = (int)Math.Round(((ans.y + clip) / (2 * clip)) * height);

            return new Tuple<bool, int, int, double>(valid, px, py, PC.length);

        }
    }
}
