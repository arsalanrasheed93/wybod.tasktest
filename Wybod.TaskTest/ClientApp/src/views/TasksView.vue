<template>
  <div>
    <div v-if="loading" class="text-center py-8 text-lg text-gray-600">
      Loading tasks...
    </div>

    <div v-else-if="error" class="text-center py-8 text-lg text-red-600">
      {{ error }}
    </div>

    <div v-else>
      <div v-if="tasks.length === 0" class="text-center py-8 text-lg text-gray-500">
        No tasks yet
      </div>

      <div v-else class="space-y-4">
        <!-- Add Task button -->
        <div class="flex justify-end">
          <button @click="showAddTaskModal = true" class="px-3 py-1 rounded-md bg-indigo-600 text-white text-sm cursor-pointer">
            Add Task
          </button>
        </div>

        <FilterTasks @change="onFilterChange" />

        <div v-if="filteredTasks.length === 0" class="text-center py-4 text-gray-500">
          No tasks match filters
        </div>

        <div v-else class="space-y-4">
          <Card v-for="task in filteredTasks" :key="task.id">
            <CardHeader>
              <div class="flex justify-between items-center">
                <CardTitle>{{ task.title }}</CardTitle>
                <span
                  :class="[
                    'px-3 py-1 rounded-full text-xs font-bold',
                    task.isCompleted ? 'bg-green-500 text-white' : 'bg-amber-500 text-white'
                  ]"
                >
                  {{ task.isCompleted ? 'Completed' : 'Pending' }}
                </span>
              </div>
              <CardDescription>{{ task.description }}</CardDescription>
            </CardHeader>
            <CardContent>
              <div class="flex gap-5 text-xs text-gray-500">
                <small>Created: {{ formatDate(task.createdAt) }}</small>
                <small v-if="task.completedAt">Completed: {{ formatDate(task.completedAt) }}</small>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>

    <!-- add new task modal -->
    <transition name="fade">
      <div v-if="showAddTaskModal" class="fixed inset-0 z-50 flex items-center justify-center">
        <div class="absolute inset-0 bg-black/40" @click="showAddTaskModal = false"></div>
        <div class="relative z-10 p-4">
          <AddNewTask
            @add="(task: any) => onAddTask({ ...task, completedAt: task.completedAt ?? undefined })"
            @close="showAddTaskModal = false"
          />
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import Card from '@/components/ui/Card.vue'
import CardHeader from '@/components/ui/CardHeader.vue'
import CardTitle from '@/components/ui/CardTitle.vue'
import CardDescription from '@/components/ui/CardDescription.vue'
import CardContent from '@/components/ui/CardContent.vue'
import FilterTasks from '@/components/ui/FilterTasks.vue'
import AddNewTask from '@/components/ui/AddNewTask.vue'
import { Status } from '@/lib/enums/status'

interface TaskItem {
  id: string
  title: string
  description: string
  isCompleted: boolean
  createdAt: string
  completedAt?: string
  priority?: string | null
  dueDate?: string | null
}

const tasks = ref<TaskItem[]>([])
const loading = ref(true)
const error = ref<string | null>(null)

// modal state
const showAddTaskModal = ref(false)

// filter state
const activeFilters = ref({ search: '', status: Status.All as Status })

const onFilterChange = (param: { search: string; status: Status }) => {
  activeFilters.value = param
}

const filteredTasks = computed(() => {
  const searchText = activeFilters.value.search.toLowerCase()
  return tasks.value.filter(task => {
    if (activeFilters.value.status === Status.Completed && !task.isCompleted) return false
    if (activeFilters.value.status === Status.Pending && task.isCompleted) return false
    if (searchText) {
      const title = task.title?.toLowerCase() ?? ''
      const desc = task.description?.toLowerCase() ?? ''
      if (!title.includes(searchText) && !desc.includes(searchText)) return false
    }
    return true
  })
})

// handler for add from modal
const onAddTask = (task: TaskItem) => {
  tasks.value.unshift(task)
  showAddTaskModal.value = false
}

const fetchTasks = async () => {
  try {
    const response = await fetch('/api/tasks')
    if (!response.ok) {
      throw new Error('Failed to fetch tasks')
    }
    tasks.value = await response.json()
  } catch (err) {
    error.value = (err as Error).message
  } finally {
    loading.value = false
  }
}

const formatDate = (dateString: string): string => {
  return new Date(dateString).toLocaleString()
}

onMounted(() => {
  fetchTasks()
})
</script>
