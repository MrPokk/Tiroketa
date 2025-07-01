using System;
using System.Collections.Generic;
using System.Reflection;

namespace Engine_Component.Utility
{
 public static class ReflectionUtility
 {
  public static List<Type> FindAllImplement<T>()
  {
   var type = typeof(T);

   var listType = new List<Type>();

   var assembly = Assembly.GetAssembly(type);

   foreach (var elementType in assembly.GetTypes())
   {
    if (elementType.IsSubclassOf(type) && !elementType.IsAbstract)
     listType.Add(elementType);
   }
   return listType;
  }
 }
}