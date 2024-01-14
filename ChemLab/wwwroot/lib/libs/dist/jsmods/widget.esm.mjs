import exporter from "../kekule.moduleEnvInits.esm.mjs";
import "../kekule.js";
import "../mins/root.min.js";
import "../mins/localization.min.js";
import "../mins/common.min.js";
import "../mins/html.min.js";
import "../mins/widget.min.js";
let { Kekule, Class, ClassEx, ObjectEx, DataType} = exporter();
export { Kekule, Class, ClassEx, ObjectEx, DataType};
if(!Kekule.scriptSrcInfo.modules)Kekule.scriptSrcInfo.modules=[];
Kekule.ArrayUtils.pushUnique(Kekule.scriptSrcInfo.modules, ["lan", "root", "localization", "localizationData", "common", "html", "widget"]);
if (typeof(Kekule) !== 'undefined') { Kekule._loaded(); }